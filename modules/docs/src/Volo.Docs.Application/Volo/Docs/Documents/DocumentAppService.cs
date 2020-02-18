using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volo.Abp.Caching;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class DocumentAppService : DocsAppServiceBase, IDocumentAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentSourceFactory _documentStoreFactory;
        protected IDistributedCache<LanguageConfig> LanguageCache { get; }
        protected IDistributedCache<DocumentResourceDto> ResourceCache { get; }
        protected IDistributedCache<DocumentUpdateInfo> DocumentUpdateCache { get; }
        protected IHostEnvironment HostEnvironment { get; }
        public DocumentAppService(
            IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            IDocumentSourceFactory documentStoreFactory,
            IDistributedCache<LanguageConfig> languageCache,
            IDistributedCache<DocumentResourceDto> resourceCache,
            IDistributedCache<DocumentUpdateInfo> documentUpdateCache,
            IHostEnvironment hostEnvironment)
        {
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _documentStoreFactory = documentStoreFactory;
            LanguageCache = languageCache;
            ResourceCache = resourceCache;
            DocumentUpdateCache = documentUpdateCache;
            HostEnvironment = hostEnvironment;
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

        public virtual async Task<NavigationNode> GetNavigationAsync(GetNavigationDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            var navigationDocument = await GetDocumentWithDetailsDtoAsync(
                project,
                project.NavigationDocumentName,
                input.LanguageCode,
                input.Version
            );

            var navigationNode = JsonConvert.DeserializeObject<NavigationNode>(navigationDocument.Content);

            var leafs = navigationNode.Items.GetAllNodes(x => x.Items)
                .Where(x => !x.Path.IsNullOrWhiteSpace())
                .ToList();

            foreach (var leaf in leafs)
            {
                var cacheKey = $"DocumentUpdateInfo{project.Id}#{leaf.Path}#{input.LanguageCode}#{input.Version}";
                var documentUpdateInfo = await DocumentUpdateCache.GetAsync(cacheKey);
                if (documentUpdateInfo != null)
                {
                    leaf.CreationTime = documentUpdateInfo.CreationTime;
                    leaf.LastUpdatedTime = documentUpdateInfo.LastUpdatedTime;
                }
            }

            return navigationNode;
        }

        public async Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);
            var cacheKey = $"Resource@{project.ShortName}#{input.LanguageCode}#{input.Name}#{input.Version}";
            input.Version = string.IsNullOrWhiteSpace(input.Version) ? project.LatestVersionBranchName : input.Version;

            async Task<DocumentResourceDto> GetResourceAsync()
            {
                var source = _documentStoreFactory.Create(project.DocumentStoreType);
                var documentResource = await source.GetResource(project, input.Name, input.LanguageCode, input.Version);

                return ObjectMapper.Map<DocumentResource, DocumentResourceDto>(documentResource);
            }

            if (HostEnvironment.IsDevelopment())
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

        public async Task<DocumentParametersDto> GetParametersAsync(GetParametersDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            try
            {
                if (string.IsNullOrWhiteSpace(project.ParametersDocumentName))
                {
                    return await Task.FromResult<DocumentParametersDto>(null);
                }

                var document = await GetDocumentWithDetailsDtoAsync(
                    project,
                    project.ParametersDocumentName,
                    input.LanguageCode,
                    input.Version
                );

                return JsonConvert.DeserializeObject<DocumentParametersDto>(document.Content);
            }
            catch (DocumentNotFoundException)
            {
                Logger.LogWarning($"Parameter file ({project.ParametersDocumentName}) not found.");
                return new DocumentParametersDto();
            }
        }

        protected virtual async Task<DocumentWithDetailsDto> GetDocumentWithDetailsDtoAsync(
            Project project,
            string documentName,
            string languageCode,
            string version)
        {
            version = string.IsNullOrWhiteSpace(version) ? project.LatestVersionBranchName : version;

            async Task<DocumentWithDetailsDto> GetDocumentAsync()
            {
                Logger.LogInformation($"Not found in the cache. Requesting {documentName} from the source...");

                var source = _documentStoreFactory.Create(project.DocumentStoreType);
                var sourceDocument = await source.GetDocumentAsync(project, documentName, languageCode, version);

                await _documentRepository.DeleteAsync(project.Id, sourceDocument.Name, sourceDocument.LanguageCode, sourceDocument.Version);
                await _documentRepository.InsertAsync(sourceDocument, true);

                Logger.LogInformation($"Document retrieved: {documentName}");

                var cacheKey = $"DocumentUpdateInfo{sourceDocument.ProjectId}#{sourceDocument.Name}#{sourceDocument.LanguageCode}#{sourceDocument.Version}";
                await DocumentUpdateCache.SetAsync(cacheKey, new DocumentUpdateInfo
                {
                    Name = sourceDocument.Name,
                    CreationTime = sourceDocument.CreationTime,
                    LastUpdatedTime = sourceDocument.LastUpdatedTime
                });

                return CreateDocumentWithDetailsDto(project, sourceDocument);
            }

            if (HostEnvironment.IsDevelopment())
            {
                return await GetDocumentAsync();
            }

            var document = await _documentRepository.FindAsync(project.Id, documentName, languageCode, version);
            if (document == null)
            {
                return await GetDocumentAsync();
            }

            //Only the latest version (dev) of the document needs to update the cache.
            if (!project.LatestVersionBranchName.IsNullOrWhiteSpace() &&
                document.Version == project.LatestVersionBranchName &&
                //TODO: Configurable cache time?
                document.LastCachedTime + TimeSpan.FromHours(2) < DateTime.Now)
            {
                return await GetDocumentAsync();
            }

            var cacheKey = $"DocumentUpdateInfo{document.ProjectId}#{document.Name}#{document.LanguageCode}#{document.Version}";
            await DocumentUpdateCache.SetAsync(cacheKey, new DocumentUpdateInfo
            {
                Name = document.Name,
                CreationTime = document.CreationTime,
                LastUpdatedTime = document.LastUpdatedTime,
            });

            return CreateDocumentWithDetailsDto(project, document);
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