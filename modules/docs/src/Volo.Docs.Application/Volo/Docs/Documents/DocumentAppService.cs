using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Docs.Caching;
using Volo.Docs.Documents.FullSearch.Elastic;
using Volo.Docs.Projects;
using Volo.Extensions;

namespace Volo.Docs.Documents
{
    public class DocumentAppService : DocsAppServiceBase, IDocumentAppService
    {
        public INavigationTreePostProcessor NavigationTreePostProcessor { get; set; }

        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentSourceFactory _documentStoreFactory;
        protected IDistributedCache<DocumentResource> ResourceCache { get; }
        protected IDistributedCache<DocumentUpdateInfo> DocumentUpdateCache { get; }
        protected IHostEnvironment HostEnvironment { get; }
        private readonly IDocumentFullSearch _documentFullSearch;
        private readonly DocsElasticSearchOptions _docsElasticSearchOptions;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _cacheTimeout;
        private readonly TimeSpan _documentResourceAbsoluteExpiration;
        private readonly TimeSpan _documentResourceSlidingExpiration;

        public DocumentAppService(
            IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            IDocumentSourceFactory documentStoreFactory,
            IDistributedCache<DocumentResource> resourceCache,
            IDistributedCache<DocumentUpdateInfo> documentUpdateCache,
            IHostEnvironment hostEnvironment,
            IDocumentFullSearch documentFullSearch,
            IOptions<DocsElasticSearchOptions> docsElasticSearchOptions,
            IConfiguration configuration)
        {
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _documentStoreFactory = documentStoreFactory;
            ResourceCache = resourceCache;
            DocumentUpdateCache = documentUpdateCache;
            HostEnvironment = hostEnvironment;
            _documentFullSearch = documentFullSearch;
            _configuration = configuration;
            _docsElasticSearchOptions = docsElasticSearchOptions.Value;
            _cacheTimeout = GetCacheTimeout();
            _documentResourceAbsoluteExpiration = GetDocumentResourceAbsoluteExpirationTimeout();
            _documentResourceSlidingExpiration = GetDocumentResourceSlidingExpirationTimeout();

            NavigationTreePostProcessor = NullNavigationTreePostProcessor.Instance;
        }

        public virtual async Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            input.Version = GetProjectVersionPrefixIfExist(project) + input.Version;

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

            input.Version = GetProjectVersionPrefixIfExist(project) + input.Version;

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

            input.Version = GetProjectVersionPrefixIfExist(project) + input.Version;

            var navigationDocument = await GetDocumentWithDetailsDtoAsync(
                project,
                project.NavigationDocumentName,
                input.LanguageCode,
                input.Version
            );

            if (!DocsJsonSerializerHelper.TryDeserialize<NavigationNode>(navigationDocument.Content,
                out var navigationNode))
            {
                throw new UserFriendlyException(
                    $"Cannot validate navigation file '{project.NavigationDocumentName}' for the project {project.Name}.");
            }

            var leafs = navigationNode.Items.GetAllNodes(x => x.Items)
                .Where(x => !x.Path.IsNullOrWhiteSpace())
                .ToList();

            foreach (var leaf in leafs)
            {
                var cacheKey =
                    CacheKeyGenerator.GenerateDocumentUpdateInfoCacheKey(project, leaf.Path, input.LanguageCode,
                        input.Version);
                var documentUpdateInfo = await DocumentUpdateCache.GetAsync(cacheKey);
                if (documentUpdateInfo != null)
                {
                    leaf.CreationTime = documentUpdateInfo.CreationTime;
                    leaf.LastUpdatedTime = documentUpdateInfo.LastUpdatedTime;
                    leaf.LastSignificantUpdateTime = documentUpdateInfo.LastSignificantUpdateTime;
                }
            }

            await NavigationTreePostProcessor.ProcessAsync(
                new NavigationTreePostProcessorContext(
                    navigationDocument,
                    navigationNode
                )
            );

            return navigationNode;
        }

        public async Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            input.Version = string.IsNullOrWhiteSpace(input.Version) ? project.LatestVersionBranchName : input.Version;
            input.Version = GetProjectVersionPrefixIfExist(project) + input.Version;

            var cacheKey =
                CacheKeyGenerator.GenerateDocumentResourceCacheKey(project, input.Name, input.LanguageCode,
                    input.Version);

            async Task<DocumentResource> GetResourceAsync()
            {
                var source = _documentStoreFactory.Create(project.DocumentStoreType);
                return await source.GetResource(project, input.Name, input.LanguageCode, input.Version);
            }

            if (HostEnvironment.IsDevelopment())
            {
                return ObjectMapper.Map<DocumentResource, DocumentResourceDto>(await GetResourceAsync());
            }

            return ObjectMapper.Map<DocumentResource, DocumentResourceDto>(
                await ResourceCache.GetOrAddAsync(
                    cacheKey,
                    GetResourceAsync,
                    () => new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = _documentResourceAbsoluteExpiration,
                        SlidingExpiration = _documentResourceSlidingExpiration
                    }
                )
            );
        }

        public async Task<List<DocumentSearchOutput>> SearchAsync(DocumentSearchInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            input.Version = GetProjectVersionPrefixIfExist(project) + input.Version;

            var esDocs =
                await _documentFullSearch.SearchAsync(input.Context, project.Id, input.LanguageCode, input.Version);

            return esDocs.Select(esDoc => new DocumentSearchOutput //TODO: auto map
                {
                    Name = esDoc.Name,
                    FileName = esDoc.FileName,
                    Version = esDoc.Version,
                    LanguageCode = esDoc.LanguageCode,
                    Highlight = esDoc.Highlight
                }).Where(x =>
                    x.FileName != project.NavigationDocumentName && x.FileName != project.ParametersDocumentName)
                .ToList();
        }

        public async Task<bool> FullSearchEnabledAsync()
        {
            return await Task.FromResult(_docsElasticSearchOptions.Enable);
        }

        public async Task<List<string>> GetUrlsAsync(string prefix)
        {
            var documentUrls = new List<string>();
            var projects = await _projectRepository.GetListAsync();

            foreach (var project in projects)
            {
                var documents = await _documentRepository.GetListByProjectId(project.Id);

                foreach (var document in documents)
                {
                    try
                    {
                        await AddDocumentToUrls(prefix, project, document, documentUrls);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }
            }

            return documentUrls;
        }

        private async Task AddDocumentToUrls(string prefix, Project project, Document document,
            List<string> documentUrls)
        {
            var navigationNodes = await GetNavigationNodesAsync(prefix, project, document);
            AddDocumentUrls(prefix, navigationNodes, documentUrls, project, document);
        }

        private void AddDocumentUrls(string prefix,
            List<NavigationNode> navigationNodes,
            List<string> documentUrls,
            Project project,
            Document document)
        {
            navigationNodes?.ForEach(node =>
            {
                documentUrls.AddIfNotContains(
                    GetDocumentLinks(node, documentUrls, prefix, project.ShortName, document)
                );
            });
        }

        private async Task<List<NavigationNode>> GetNavigationNodesAsync(string prefix, Project project,
            Document document)
        {
            var version = GetProjectVersionPrefixIfExist(project) + document.Version;
            var navigationDocument = await GetDocumentWithDetailsDtoAsync(
                project,
                project.NavigationDocumentName,
                document.LanguageCode,
                version
            );

            if (!DocsJsonSerializerHelper.TryDeserialize<NavigationNode>(navigationDocument.Content,
                out var navigationNode))
            {
                throw new UserFriendlyException(
                    $"Cannot validate navigation file '{project.NavigationDocumentName}' for the project {project.Name}.");
            }

            return navigationNode.Items;
        }

        private List<string> GetDocumentLinks(NavigationNode node, List<string> documentUrls, string prefix,
            string shortName, Document document)
        {
            if (!IsExternalLink(node.Path))
            {
                documentUrls.AddIfNotContains(
                    NormalizePath(prefix, node.Path, shortName, document)
                );
            }

            node.Items?.ForEach(childNode =>
            {
                GetDocumentLinks(childNode, documentUrls, prefix, shortName, document);
            });

            return documentUrls;
        }

        private string NormalizePath(string prefix, string path, string shortName, Document document)
        {
            var pathWithoutFileExtension = RemoveFileExtensionFromPath(path, document.Format);
            var normalizedPath = prefix + document.LanguageCode + "/" + shortName + "/" + document.Version + "/" +
                                 pathWithoutFileExtension;

            return normalizedPath;
        }

        private string RemoveFileExtensionFromPath(string path, string format)
        {
            if (path == null)
            {
                return null;
            }

            return path.EndsWith("." + format)
                ? path.Left(path.Length - format.Length - 1)
                : path;
        }

        private static bool IsExternalLink(string path)
        {
            if (path.IsNullOrEmpty())
            {
                return false;
            }

            return path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<DocumentParametersDto> GetParametersAsync(GetParametersDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            input.Version = GetProjectVersionPrefixIfExist(project) + input.Version;

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

                if (!DocsJsonSerializerHelper.TryDeserialize<DocumentParametersDto>(document.Content,
                    out var documentParameters))
                {
                    throw new UserFriendlyException(
                        $"Cannot validate document parameters file '{project.ParametersDocumentName}' for the project {project.Name}.");
                }

                return documentParameters;
            }
            catch (DocumentNotFoundException)
            {
                Logger.LogWarning($"Parameter file ({project.ParametersDocumentName}) not found!");
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

            if (HostEnvironment.IsDevelopment())
            {
                return await GetDocumentAsync(documentName, project, languageCode, version);
            }

            var document = await _documentRepository.FindAsync(project.Id, documentName, languageCode, version);
            if (document == null)
            {
                return await GetDocumentAsync(documentName, project, languageCode, version);
            }

            if (document.LastCachedTime + _cacheTimeout < DateTime.Now)
            {
                return await GetDocumentAsync(documentName, project, languageCode, version, document);
            }

            var cacheKey = CacheKeyGenerator.GenerateDocumentUpdateInfoCacheKey(
                project,
                document.Name,
                document.LanguageCode,
                document.Version
            );

            await DocumentUpdateCache.SetAsync(cacheKey, new DocumentUpdateInfo
            {
                Name = document.Name,
                CreationTime = document.CreationTime,
                LastUpdatedTime = document.LastUpdatedTime,
                LastSignificantUpdateTime = document.LastSignificantUpdateTime
            });

            return CreateDocumentWithDetailsDto(project, document);
        }

        protected virtual DocumentWithDetailsDto CreateDocumentWithDetailsDto(Project project, Document document)
        {
            var documentDto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);

            documentDto.Project = ObjectMapper.Map<Project, ProjectDto>(project);
            documentDto.Contributors =
                ObjectMapper.Map<List<DocumentContributor>, List<DocumentContributorDto>>(document.Contributors);

            return documentDto;
        }

        private async Task<DocumentWithDetailsDto> GetDocumentAsync(string documentName, Project project,
            string languageCode, string version, Document oldDocument = null)
        {
            Logger.LogInformation($"Not found in the cache. Requesting {documentName} from the source...");

            var source = _documentStoreFactory.Create(project.DocumentStoreType);
            var sourceDocument = await source.GetDocumentAsync(project, documentName, languageCode, version,
                oldDocument?.LastSignificantUpdateTime);

            await _documentRepository.DeleteAsync(project.Id, sourceDocument.Name, sourceDocument.LanguageCode,
                sourceDocument.Version);
            await _documentRepository.InsertAsync(sourceDocument, true);

            Logger.LogInformation($"Document retrieved: {documentName}");

            var cacheKey = CacheKeyGenerator.GenerateDocumentUpdateInfoCacheKey(
                project,
                sourceDocument.Name,
                sourceDocument.LanguageCode,
                sourceDocument.Version
            );

            await DocumentUpdateCache.SetAsync(cacheKey, new DocumentUpdateInfo
            {
                Name = sourceDocument.Name,
                CreationTime = sourceDocument.CreationTime,
                LastUpdatedTime = sourceDocument.LastUpdatedTime,
                LastSignificantUpdateTime = sourceDocument.LastSignificantUpdateTime
            });

            return CreateDocumentWithDetailsDto(project, sourceDocument);
        }

        private TimeSpan GetCacheTimeout()
        {
            var value = _configuration["Volo.Docs:DocumentCacheTimeoutInterval"];
            if (value.IsNullOrEmpty())
            {
                return TimeSpan.FromHours(6);
            }

            return TimeSpan.Parse(value);
        }

        private TimeSpan GetDocumentResourceAbsoluteExpirationTimeout()
        {
            var value = _configuration["Volo.Docs:DocumentResource.AbsoluteExpirationRelativeToNow"];
            if (value.IsNullOrEmpty())
            {
                return TimeSpan.FromHours(6);
            }

            return TimeSpan.Parse(value);
        }

        private TimeSpan GetDocumentResourceSlidingExpirationTimeout()
        {
            var value = _configuration["Volo.Docs:DocumentResource.SlidingExpiration"];
            if (value.IsNullOrEmpty())
            {
                return TimeSpan.FromMinutes(30);
            }

            return TimeSpan.Parse(value);
        }

        private string GetProjectVersionPrefixIfExist(Project project)
        {
            if (GetGithubVersionProviderSource(project) != GithubVersionProviderSource.Branches)
            {
                return string.Empty;
            }

            return project.ExtraProperties["VersionBranchPrefix"].ToString();
        }

        private GithubVersionProviderSource GetGithubVersionProviderSource(Project project)
        {
            return project.ExtraProperties.ContainsKey("GithubVersionProviderSource")
                ? (GithubVersionProviderSource) (long) project.ExtraProperties["GithubVersionProviderSource"]
                : GithubVersionProviderSource.Releases;
        }
    }
}