using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Docs.Caching;
using Volo.Docs.Documents;
using Volo.Docs.Documents.FullSearch.Elastic;
using Volo.Docs.Localization;
using Volo.Docs.Projects;

namespace Volo.Docs.Admin.Documents
{
    [Authorize(DocsAdminPermissions.Documents.Default)]
    public class DocumentAdminAppService : ApplicationService, IDocumentAdminAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDistributedCache<DocumentUpdateInfo> _documentUpdateCache;
        private readonly IDistributedCache<List<VersionInfo>> _versionCache;
        private readonly IDistributedCache<LanguageConfig> _languageCache;
        private readonly IDocumentFullSearch _documentFullSearch;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public DocumentAdminAppService(IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            IDistributedCache<DocumentUpdateInfo> documentUpdateCache,
            IDistributedCache<List<VersionInfo>> versionCache,
            IDistributedCache<LanguageConfig> languageCache,
            IDocumentFullSearch documentFullSearch,
            IBackgroundJobManager backgroundJobManager)
        {
            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _documentUpdateCache = documentUpdateCache;
            _versionCache = versionCache;
            _languageCache = languageCache;
            _documentFullSearch = documentFullSearch;
            _backgroundJobManager = backgroundJobManager;

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
            await _backgroundJobManager.EnqueueAsync(new PullAllBackgroundJob.PullBackgroundWorkerArgs(input.ProjectId, input.LanguageCode, input.Version));
        }

        public async Task PullAsync(PullDocumentInput input)
        {
            await _backgroundJobManager.EnqueueAsync(new PullAllBackgroundJob.PullBackgroundWorkerArgs(input.ProjectId, input.LanguageCode, input.Version, input.Name));
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
                Items = ObjectMapper.Map<List<DocumentWithoutContent>, List<DocumentDto>>(docs)
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
            _documentFullSearch.ValidateElasticSearchEnabled();

            await _documentFullSearch.DeleteAsync(documentId);
            var document = await _documentRepository.GetAsync(documentId);
            await _documentFullSearch.AddOrUpdateAsync(document);
        }
    }
}
