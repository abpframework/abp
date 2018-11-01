using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDistributedCache<List<VersionInfoDto>> _distributedCache;
        private readonly IDocumentStoreFactory _documentStoreFactory;

        public DocumentAppService(
            IProjectRepository projectRepository,
            IDistributedCache<List<VersionInfoDto>> distributedCache,
            IDocumentStoreFactory documentStoreFactory)
        {
            _projectRepository = projectRepository;
            _distributedCache = distributedCache;
            _documentStoreFactory = documentStoreFactory;
        }

        public async Task<DocumentWithDetailsDto> GetByNameAsync(
            string projectShortName, 
            string documentName, 
            string version, 
            bool normalize)
        {
            var project = await _projectRepository.GetByShortNameAsync(projectShortName);
            return await GetDocumentWithDetailsDto(
                project,
                documentName,
                version,
                normalize
            );
        }

        public async Task<DocumentWithDetailsDto> GetDefaultAsync(
            string projectShortName, 
            string version, 
            bool normalize)
        {
            var project = await _projectRepository.GetByShortNameAsync(projectShortName);
            return await GetDocumentWithDetailsDto(
                project,
                project.DefaultDocumentName,
                version,
                normalize
            );
        }

        public virtual async Task<NavigationWithDetailsDto> GetNavigationDocumentAsync(
            string projectShortName, 
            string version, 
            bool normalize)
        {
            var project = await _projectRepository.GetByShortNameAsync(projectShortName);
            var documentDto = await GetDocumentWithDetailsDto(
                project,
                project.NavigationDocumentName,
                version,
                normalize
            );

            return ObjectMapper.Map<DocumentWithDetailsDto, NavigationWithDetailsDto>(documentDto);
        }

        protected virtual async Task<DocumentWithDetailsDto> GetDocumentWithDetailsDto(
            Project project, 
            string documentName, 
            string version, 
            bool normalize)
        {
            var documentStore = _documentStoreFactory.Create(project.DocumentStoreType);
            var document = await documentStore.Find(project, documentName, version);

            var dto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);
            dto.Project = ObjectMapper.Map<Project, ProjectDto>(project);

            return dto;
        }

        public async Task<List<VersionInfoDto>> GetVersions(
            string projectShortName
            )
        {
            var project = await _projectRepository.GetByShortNameAsync(projectShortName);

            var documentStore = _documentStoreFactory.Create(project.DocumentStoreType);

            var versions = await GetVersionsFromCache(projectShortName);

            if (versions == null)
            {
                versions = await documentStore.GetVersions(project);
                await SetVersionsToCache(projectShortName, versions);
            }

            if (!project.MinimumVersion.IsNullOrEmpty())
            {
                var minVersionIndex = versions.FindIndex(v => v.Name == project.MinimumVersion);
                if (minVersionIndex > -1)
                {
                    versions = versions.GetRange(0, minVersionIndex + 1);
                }
            }

            if (!string.IsNullOrEmpty(project.LatestVersionBranchName))
            {
                versions.First().Name = project.LatestVersionBranchName;
            }
            
            return versions;
        }

        private async Task<List<VersionInfoDto>> GetVersionsFromCache(string projectShortName)
        {
            return await _distributedCache.GetAsync(projectShortName);
        }

        private async Task SetVersionsToCache(string projectShortName, List<VersionInfoDto> versions)
        {
            var options = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromDays(1) };
            await _distributedCache.SetAsync(projectShortName, versions, options);
        }
    }
}