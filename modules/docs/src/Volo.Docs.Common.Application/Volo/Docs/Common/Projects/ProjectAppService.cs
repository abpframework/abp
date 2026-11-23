using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Docs.GitHub.Documents.Version;
using Volo.Abp.Threading;
using Volo.Docs.Caching;
using Volo.Docs.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.Common.Projects
{
    public class ProjectAppService : DocsCommonAppServiceBase, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDistributedCache<List<VersionInfo>> _versionCache;
        private readonly IDocumentSourceFactory _documentSource;
        protected IDistributedCache<LanguageConfig> LanguageCache { get; }
        protected ProjectLanguageCacheKeyProvider ProjectLanguageCacheKeyProvider { get; }

        private readonly SemaphoreSlim _syncSemaphore = new SemaphoreSlim(1, 1);

        public ProjectAppService(
            IProjectRepository projectRepository,
            IDistributedCache<List<VersionInfo>> versionCache,
            IDocumentSourceFactory documentSource,
            IDistributedCache<LanguageConfig> languageCache,
            ProjectLanguageCacheKeyProvider projectLanguageCacheKeyProvider)
        {
            _projectRepository = projectRepository;
            _versionCache = versionCache;
            _documentSource = documentSource;
            LanguageCache = languageCache;
            ProjectLanguageCacheKeyProvider = projectLanguageCacheKeyProvider;
        }

        public virtual async Task<ListResultDto<ProjectDto>> GetListAsync()
        {
            var projects = await _projectRepository.GetListAsync();

            return new ListResultDto<ProjectDto>(
                ObjectMapper.Map<List<Project>, List<ProjectDto>>(projects)
            );
        }

        public virtual async Task<ProjectDto> GetAsync(string shortName)
        {
            var project = await _projectRepository.GetByShortNameAsync(shortName);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        public virtual async Task<ListResultDto<VersionInfoDto>> GetVersionsAsync(string shortName)
        {
            var project = await _projectRepository.GetByShortNameAsync(shortName);
            var versions = await GetCachedVersionsAsync(project);
            return new ListResultDto<VersionInfoDto>(ObjectMapper.Map<List<VersionInfo>, List<VersionInfoDto>>(versions));
        }

        private async Task<List<VersionInfo>> GetCachedVersionsAsync(Project project)
        {
            using (await _syncSemaphore.LockAsync())
            {
                var versions = await _versionCache.GetAsync(CacheKeyGenerator.GenerateProjectVersionsCacheKey(project));
                if (versions.IsNullOrEmpty())
                {
                    versions = await GetVersionsAsync(project);
                    if (!versions.IsNullOrEmpty())
                    {
                        await _versionCache.SetAsync(
                            CacheKeyGenerator.GenerateProjectVersionsCacheKey(project),
                            versions,
                            new DistributedCacheEntryOptions
                            {
                                //TODO: Configurable?
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12),
                                SlidingExpiration = TimeSpan.FromMinutes(60)
                            }
                        );
                    }
                }

                return versions;
            }
        }

        protected virtual async Task<List<VersionInfo>> GetVersionsAsync(Project project)
        {
            var store = _documentSource.Create(project.DocumentStoreType);
            var versions = await store.GetVersionsAsync(project);

            if (!versions.Any())
            {
                return versions;
            }

            if (!project.MinimumVersion.IsNullOrEmpty())
            {
                var minVersionIndex = versions.FindIndex(v => v.Name == project.MinimumVersion);
                if (minVersionIndex > -1)
                {
                    versions = versions.GetRange(0, minVersionIndex + 1);
                }
            }

            return versions;
        }

        public virtual async Task<LanguageConfig> GetLanguageListAsync(string shortName, string version)
        {
            return await GetLanguageListInternalAsync(shortName, version);
        }

        public virtual async Task<string> GetDefaultLanguageCodeAsync(string shortName, string version)
        {
            var languageList = await GetLanguageListInternalAsync(shortName, version);

            return (languageList.Languages.FirstOrDefault(l => l.IsDefault) ?? languageList.Languages.First()).Code;
        }

        private async Task<LanguageConfig> GetLanguageListInternalAsync(string shortName, string version)
        {
            var project = await _projectRepository.GetByShortNameAsync(shortName);
            var store = _documentSource.Create(project.DocumentStoreType);

            var versions = await GetCachedVersionsAsync(project);
            var defaultVersion = project.LatestVersionBranchName;
            if (defaultVersion.IsNullOrWhiteSpace())
            {
                defaultVersion = versions.FirstOrDefault(x => !SemanticVersionHelper.IsPreRelease(x.Name))?.Name ?? versions.FirstOrDefault()?.Name;
            }

            version = project.GetFullVersion((version.IsNullOrWhiteSpace() || string.Equals(version, ProjectConsts.Latest, StringComparison.OrdinalIgnoreCase)
                ? defaultVersion
                : version) ?? string.Empty);

            var isKnownVersion = (!defaultVersion.IsNullOrWhiteSpace() && version == project.GetFullVersion(defaultVersion)) ||
                                 versions.Any(x => project.GetFullVersion(x.Name) == version);
            var key = await ProjectLanguageCacheKeyProvider.GetKeyAsync(project, version);
            if (key != null)
            {
                var cached = await LanguageCache.GetAsync(key);
                if (cached != null)
                {
                    return cached;
                }
            }

            if (!isKnownVersion && (versions.Any() || (!version.IsNullOrEmpty() && !DocumentVersionHelper.IsValidUnlistedVersionName(version))))
            {
                throw new EntityNotFoundException(typeof(VersionInfo), version);
            }

            var languages = await store.GetLanguageListAsync(project, version);
            // Unlisted labels may be readable, but must not create language cache entries.
            if (isKnownVersion && key != null && languages is not FallbackLanguageConfig)
            {
                await LanguageCache.SetAsync(key, languages, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                });
            }

            return languages;
        }
    }
}
