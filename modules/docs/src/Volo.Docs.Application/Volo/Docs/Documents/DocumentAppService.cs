using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class DocumentAppService : DocsAppServiceBase, IDocumentAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentStoreFactory _documentStoreFactory;
        protected IDistributedCache<DocumentWithDetailsDto> DocumentCache { get; }
        protected IDistributedCache<LanguageConfig> LanguageCache { get; }
        protected IDistributedCache<DocumentResourceDto> ResourceCache { get; }

        public DocumentAppService(
            IProjectRepository projectRepository,
            IDocumentStoreFactory documentStoreFactory,
            IDistributedCache<DocumentWithDetailsDto> documentCache,
            IDistributedCache<LanguageConfig> languageCache,
            IDistributedCache<DocumentResourceDto> resourceCache)
        {
            _projectRepository = projectRepository;
            _documentStoreFactory = documentStoreFactory;
            DocumentCache = documentCache;
            LanguageCache = languageCache;
            ResourceCache = resourceCache;
        }

        public virtual async Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDtoAsync(
                project,
                input.Name,
                input.LanguageCode,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDtoAsync(
                project,
                project.DefaultDocumentName + "." + project.Format,
                input.LanguageCode,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetNavigationAsync(GetNavigationDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDtoAsync(
                project,
                project.NavigationDocumentName,
                input.LanguageCode,
                input.Version
            );
        }

        public async Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);
            var cacheKey = $"Resource@{project.ShortName}#{input.LanguageCode}#{input.Name}#{input.Version}";
            input.Version = string.IsNullOrWhiteSpace(input.Version) ? project.LatestVersionBranchName : input.Version;

            async Task<DocumentResourceDto> GetResourceAsync()
            {
                var store = _documentStoreFactory.Create(project.DocumentStoreType);
                var documentResource = await store.GetResource(project, input.Name, input.LanguageCode, input.Version);

                return ObjectMapper.Map<DocumentResource, DocumentResourceDto>(documentResource);
            }

            if (Debugger.IsAttached)
            {
                return await GetResourceAsync();
            }

            return await ResourceCache.GetOrAddAsync(
                cacheKey,
                GetResourceAsync,
                () => new DistributedCacheEntryOptions
                {
                    //TODO: Configurable?
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                }
            );
        }

        protected virtual async Task<DocumentWithDetailsDto> GetDocumentWithDetailsDtoAsync(
            Project project,
            string documentName,
            string languageCode,
            string version)
        {
            version = string.IsNullOrWhiteSpace(version) ? project.LatestVersionBranchName : version;

            var cacheKey = $"Document@{project.ShortName}#{languageCode}#{documentName}#{version}";

            async Task<DocumentWithDetailsDto> GetDocumentAsync()
            {
                Logger.LogInformation($"Not found in the cache. Requesting {documentName} from the store...");
                var store = _documentStoreFactory.Create(project.DocumentStoreType);
                var document = await store.GetDocumentAsync(project, documentName, languageCode, version);
                Logger.LogInformation($"Document retrieved: {documentName}");
                return CreateDocumentWithDetailsDto(project, document);
            }

            if (Debugger.IsAttached)
            {
                return await GetDocumentAsync();
            }

            return await DocumentCache.GetOrAddAsync(
                cacheKey,
                GetDocumentAsync,
                () => new DistributedCacheEntryOptions
                {
                    //TODO: Configurable?
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                }
            );
        }

        protected virtual DocumentWithDetailsDto CreateDocumentWithDetailsDto(Project project, Document document)
        {
            var documentDto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);
            documentDto.Project = ObjectMapper.Map<Project, ProjectDto>(project);
            documentDto.Contributors = ObjectMapper.Map<List<DocumentContributor>, List<DocumentContributorDto>>(document.Contributors);
            return documentDto;
        }
    }
}