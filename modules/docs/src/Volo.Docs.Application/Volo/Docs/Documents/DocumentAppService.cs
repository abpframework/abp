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
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentStoreFactory _documentStoreFactory;
        protected IDistributedCache<DocumentWithDetailsDto> DocumentCache { get; }
        protected IDistributedCache<LanguageConfig> LanguaCache { get; }
        protected IDistributedCache<DocumentResourceDto> ResourceCache { get; }

        public DocumentAppService(
            IProjectRepository projectRepository,
            IDocumentStoreFactory documentStoreFactory,
            IDistributedCache<DocumentWithDetailsDto> documentCache,
            IDistributedCache<LanguageConfig> languaCache,
            IDistributedCache<DocumentResourceDto> resourceCache)
        {
            _projectRepository = projectRepository;
            _documentStoreFactory = documentStoreFactory;
            DocumentCache = documentCache;
            LanguaCache = languaCache;
            ResourceCache = resourceCache;
        }

        public virtual async Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocument(
                project,
                input.Name,
                input.LanguageCode,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocument(
                project,
                project.DefaultDocumentName,
                input.LanguageCode,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetNavigationAsync(GetNavigationDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocument(
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

        protected virtual async Task<DocumentWithDetailsDto> GetDocument(
            Project project,
            string documentName,
            string languageCode,
            string version)
        {
            var cacheKey = $"Document@{project.ShortName}#{languageCode}#{documentName}#{version}";

            async Task<DocumentWithDetailsDto> GetDocumentAsync()
            {
                Logger.LogInformation($"Not found in the cache. Requesting {documentName} from the store...");
                var store = _documentStoreFactory.Create(project.DocumentStoreType);
                var languages = await GetLanguageListAsync(store, project, version);
                var language = GetLanguageByCode(languages, languageCode);
                var document = await store.GetDocumentAsync(project, documentName, language.Code, version);
                Logger.LogInformation($"Document retrieved: {documentName}");
                return CreateDocumentWithDetailsDto(project, document, languages, language.Code);
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

        protected virtual LanguageInfo GetLanguageByCode(LanguageConfig languageCodes, string languageCode)
        {
            var language = languageCodes.Languages.FirstOrDefault(l => l.Code == languageCode);

            return language ?? languageCodes.Languages.Single(l => l.IsDefault);
        }

        protected virtual async Task<LanguageConfig> GetLanguageListAsync(IDocumentStore store, Project project, string version)
        {
            async Task<LanguageConfig> GetLanguagesAsync()
            {
                return await store.GetLanguageListAsync(project, version);
            }

            return await LanguaCache.GetOrAddAsync(
                project.ShortName,
                GetLanguagesAsync,
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                }
            );
        }

        protected virtual DocumentWithDetailsDto CreateDocumentWithDetailsDto(Project project, Document document, LanguageConfig languages, string languageCode)
        {
            var documentDto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);
            documentDto.Project = ObjectMapper.Map<Project, ProjectDto>(project);
            documentDto.Contributors = ObjectMapper.Map<List<DocumentContributor>, List<DocumentContributorDto>>(document.Contributors);

            documentDto.Project.Languages = new Dictionary<string, string>();
            foreach (var language in languages.Languages)
            {
                documentDto.Project.Languages.Add(language.Code,language.DisplayName);
            }

            documentDto.CurrentLanguageCode = languageCode;
            return documentDto;
        }
    }
}