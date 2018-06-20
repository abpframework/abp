using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDistributedCache<List<string>> _distributedCache;
        private readonly IDocumentStoreFactory _documentStoreFactory;

        public DocumentAppService(
            IProjectRepository projectRepository,
            IDistributedCache<List<string>> distributedCache,
            IDocumentStoreFactory documentStoreFactory)
        {
            _projectRepository = projectRepository;
            _distributedCache = distributedCache;
            _documentStoreFactory = documentStoreFactory;
        }

        public async Task<DocumentWithDetailsDto> GetByNameAsync(string projectShortName, string documentName, string version)
        {
            var project = await _projectRepository.FindByShortNameAsync(projectShortName);

            return await GetDocument(project, documentName, version);
        }

        public async Task<DocumentWithDetailsDto> GetNavigationDocumentAsync(string projectShortName, string version)
        {
            var project = await _projectRepository.FindByShortNameAsync(projectShortName);

            return await GetDocument(project, project.NavigationDocumentName, version);
        }

        private async Task<DocumentWithDetailsDto> GetDocument(Project project, string documentName, string version)
        {
            if (project == null)
            {
                throw new EntityNotFoundException($"Project Not Found!");
            }

            if (string.IsNullOrWhiteSpace(documentName))
            {
                documentName = project.DefaultDocumentName;
            }

            var documentStore = _documentStoreFactory.Create(project);
            var document = await documentStore.FindDocumentByNameAsync(project, documentName, version);

            var dto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);

            dto.Project = ObjectMapper.Map<Project, ProjectDto>(project);

            dto.Content = NormalizeLinks(dto.Content, project.ShortName, version);
            dto.Content = NormalizeImages(dto.Content, dto.RawRootUrl);

            return dto;
        }

        public async Task<List<string>> GetVersions(string projectShortName, string documentName)
        {
            var project = await _projectRepository.FindByShortNameAsync(projectShortName);

            if (project == null)
            {
                throw new EntityNotFoundException($"Project Not Found!");
            }

            if (string.IsNullOrWhiteSpace(documentName))
            {
                documentName = project.DefaultDocumentName;
            }

            var documentStore = _documentStoreFactory.Create(project);

            var versions = await GetVersionsFromCache(projectShortName);

            if (versions == null)
            {
                versions = await documentStore.GetVersions(project, documentName);
                await SetVersionsToCache(projectShortName, versions);
            }

            return versions;
        }

        private async Task<List<string>> GetVersionsFromCache(string projectShortName)
        {
            return await _distributedCache.GetAsync(projectShortName);
        }

        private async Task SetVersionsToCache(string projectShortName, List<string> versions)
        {
            var options = new DistributedCacheEntryOptions(){SlidingExpiration = TimeSpan.FromDays(1)};
            await _distributedCache.SetAsync(projectShortName, versions, options);
        }

        private static string NormalizeLinks(string content, string projectShortName, string version)
        {
            var linkRegex = new Regex(@"\(([^)]+.md)\)", RegexOptions.Multiline);
            var matches = linkRegex.Matches(content);
            foreach (Match match in matches)
            {
                var mdFile = match.Value;
                content = content.Replace(mdFile, "(/documents/" + projectShortName + "/" + version + "/" + mdFile.Replace("(", "").Replace(")", "").Replace(".md", "") + ")");
            }

            return content;
        }

        private static string NormalizeImages(string content, string documentRootAddress)
        {
            var baseImageUrl = documentRootAddress;

            if (content.Contains("images/"))
            {
                Console.Write("");
            }

            var newSrcAttribute = "src=\"" + baseImageUrl + "images/\" data-src=\"" + baseImageUrl + "images/";

            return content.Replace("src=\"images/", newSrcAttribute)
                .Replace("src=\"../images/", newSrcAttribute);
        }
    }
}