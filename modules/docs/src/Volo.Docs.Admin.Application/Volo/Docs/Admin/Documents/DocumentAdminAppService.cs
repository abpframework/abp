using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Uow;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Docs.Caching;
using Volo.Docs.Documents;
using Volo.Docs.Documents.FullSearch.Elastic;
using Volo.Docs.Localization;
using Volo.Docs.Projects;
using Volo.Extensions;

namespace Volo.Docs.Admin.Documents
{
    [Authorize(DocsAdminPermissions.Documents.Default)]
    public class DocumentAdminAppService : ApplicationService, IDocumentAdminAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentSourceFactory _documentStoreFactory;
        private readonly IDistributedCache<DocumentUpdateInfo> _documentUpdateCache;
        private readonly IDistributedCache<List<VersionInfo>> _versionCache;
        private readonly IDistributedCache<LanguageConfig> _languageCache;
        private readonly IDocumentFullSearch _elasticSearchService;

        public DocumentAdminAppService(IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            IDocumentSourceFactory documentStoreFactory,
            IDistributedCache<DocumentUpdateInfo> documentUpdateCache,
            IDistributedCache<List<VersionInfo>> versionCache,
            IDistributedCache<LanguageConfig> languageCache,
            IDocumentFullSearch elasticSearchService)
        {
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _documentStoreFactory = documentStoreFactory;
            _documentUpdateCache = documentUpdateCache;
            _versionCache = versionCache;
            _languageCache = languageCache;
            _elasticSearchService = elasticSearchService;

            LocalizationResource = typeof(DocsResource);
        }

        public async Task ClearCacheAsync(ClearCacheInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            var languageCacheKey = CacheKeyGenerator.GenerateProjectLanguageCacheKey(project);
            await _languageCache.RemoveAsync(languageCacheKey, true);

            var versionCacheKey = CacheKeyGenerator.GenerateProjectVersionsCacheKey(project);
            await _versionCache.RemoveAsync(versionCacheKey, true);

            var documents = await _documentRepository.GetListByProjectId(project.Id);

            foreach (var document in documents)
            {
                var documentUpdateInfoCacheKey = CacheKeyGenerator.GenerateDocumentUpdateInfoCacheKey(
                    project: project,
                    documentName: document.Name,
                    languageCode: document.LanguageCode,
                    version: document.Version
                );

                await _documentUpdateCache.RemoveAsync(documentUpdateInfoCacheKey);

                document.LastCachedTime = DateTime.MinValue;
                await _documentRepository.UpdateAsync(document);
            }
        }

        public async Task PullAllAsync(PullAllDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            var navigationDocument = await GetDocumentAsync(
                project,
                project.NavigationDocumentName,
                input.LanguageCode,
                input.Version
            );

            if (!DocsJsonSerializerHelper.TryDeserialize<NavigationNode>(navigationDocument.Content, out var navigation))
            {
                throw new UserFriendlyException($"Cannot validate navigation file '{project.NavigationDocumentName}' for the project {project.Name}.");
            }

            var leafs = navigation.Items.GetAllNodes(x => x.Items)
                .Where(x => x.IsLeaf && !x.Path.IsNullOrWhiteSpace())
                .ToList();

            var source = _documentStoreFactory.Create(project.DocumentStoreType);

            var documents = new List<Document>();
            foreach (var leaf in leafs)
            {
                if (leaf.Path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    leaf.Path.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                    (leaf.Path.StartsWith("{{") && leaf.Path.EndsWith("}}")))
                {
                    continue;
                }

                try
                {
                    var sourceDocument = await source.GetDocumentAsync(project, leaf.Path, input.LanguageCode, input.Version);
                    documents.Add(sourceDocument);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }

            foreach (var document in documents)
            {
                await _documentRepository.DeleteAsync(document.ProjectId, document.Name,
                    document.LanguageCode,
                    document.Version);

                await _documentRepository.InsertAsync(document, true);
                await UpdateDocumentUpdateInfoCache(document);
            }
        }

        public async Task PullAsync(PullDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            var source = _documentStoreFactory.Create(project.DocumentStoreType);
            var sourceDocument = await source.GetDocumentAsync(project, input.Name, input.LanguageCode, input.Version);

            await _documentRepository.DeleteAsync(sourceDocument.ProjectId, sourceDocument.Name,
                sourceDocument.LanguageCode, sourceDocument.Version);
            await _documentRepository.InsertAsync(sourceDocument, true);
            await UpdateDocumentUpdateInfoCache(sourceDocument);
        }

        public async Task<PagedResultDto<DocumentDto>> GetAllAsync(GetAllInput input)
        {
            var totalCount = await _documentRepository.GetAllCountAsync(
                projectId: input.ProjectId,
                name: input.Name,
                version: input.Version,
                languageCode: input.LanguageCode,
                fileName: input.FileName,
                format: input.Format,
                creationTimeMin: input.CreationTimeMin,
                creationTimeMax: input.CreationTimeMax,
                lastUpdatedTimeMin: input.LastUpdatedTimeMin,
                lastUpdatedTimeMax: input.LastUpdatedTimeMax,
                lastSignificantUpdateTimeMin: input.LastSignificantUpdateTimeMin,
                lastSignificantUpdateTimeMax: input.LastSignificantUpdateTimeMax,
                lastCachedTimeMin: input.LastCachedTimeMin,
                lastCachedTimeMax: input.LastCachedTimeMax,
                sorting: input.Sorting,
                maxResultCount: input.MaxResultCount,
                skipCount: input.SkipCount
            );

            var docs = await _documentRepository.GetAllAsync(
                projectId: input.ProjectId,
                name: input.Name,
                version: input.Version,
                languageCode: input.LanguageCode,
                fileName: input.FileName,
                format: input.Format,
                creationTimeMin: input.CreationTimeMin,
                creationTimeMax: input.CreationTimeMax,
                lastUpdatedTimeMin: input.LastUpdatedTimeMin,
                lastUpdatedTimeMax: input.LastUpdatedTimeMax,
                lastSignificantUpdateTimeMin: input.LastSignificantUpdateTimeMin,
                lastSignificantUpdateTimeMax: input.LastSignificantUpdateTimeMax,
                lastCachedTimeMin: input.LastCachedTimeMin,
                lastCachedTimeMax: input.LastCachedTimeMax,
                sorting: input.Sorting,
                maxResultCount: input.MaxResultCount,
                skipCount: input.SkipCount
            );

            return new PagedResultDto<DocumentDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Document>, List<DocumentDto>>(docs)
            };
        }

        public async Task RemoveFromCacheAsync(Guid documentId)
        {
            var document = await _documentRepository.GetAsync(documentId);
            var project = await _projectRepository.GetAsync(document.ProjectId);

            var documentUpdateInfoCacheKey = CacheKeyGenerator.GenerateDocumentUpdateInfoCacheKey(
                project: project,
                documentName: document.Name,
                languageCode: document.LanguageCode,
                version: document.Version
            );

            await _documentUpdateCache.RemoveAsync(documentUpdateInfoCacheKey);
            await _documentRepository.DeleteAsync(document);
        }

        public async Task ReindexAsync(Guid documentId)
        {
            _elasticSearchService.ValidateElasticSearchEnabled();

            await _elasticSearchService.DeleteAsync(documentId);
            var document = await _documentRepository.GetAsync(documentId);
            await _elasticSearchService.AddOrUpdateAsync(document);
        }

        private async Task UpdateDocumentUpdateInfoCache(Document document)
        {
            var cacheKey = $"DocumentUpdateInfo{document.ProjectId}#{document.Name}#{document.LanguageCode}#{document.Version}";
            await _documentUpdateCache.SetAsync(cacheKey, new DocumentUpdateInfo
            {
                Name = document.Name,
                CreationTime = document.CreationTime,
                LastUpdatedTime = document.LastUpdatedTime
            });
        }

        private async Task<Document> GetDocumentAsync(
            Project project,
            string documentName,
            string languageCode,
            string version)
        {
            version = string.IsNullOrWhiteSpace(version) ? project.LatestVersionBranchName : version;
            var source = _documentStoreFactory.Create(project.DocumentStoreType);
            var document = await source.GetDocumentAsync(project, documentName, languageCode, version);
            return document;
        }
    }
}
