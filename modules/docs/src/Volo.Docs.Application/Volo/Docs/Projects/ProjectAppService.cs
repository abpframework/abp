using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Guids;
using Volo.Docs.Caching;
using Volo.Docs.Documents;

namespace Volo.Docs.Projects
{
    public class ProjectAppService : DocsAppServiceBase, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDistributedCache<List<VersionInfo>> _versionCache;
        private readonly IDocumentSourceFactory _documentSource;
        protected IDistributedCache<LanguageConfig> LanguageCache { get; }

        public ProjectAppService(
            IProjectRepository projectRepository,
            IDistributedCache<List<VersionInfo>> versionCache,
            IDocumentSourceFactory documentSource,
            IDistributedCache<LanguageConfig> languageCache)
        {
            _projectRepository = projectRepository;
            _versionCache = versionCache;
            _documentSource = documentSource;
            LanguageCache = languageCache;
        }

        public async Task<ListResultDto<ProjectDto>> GetListAsync()
        {
            var projects = await _projectRepository.GetListAsync();

            return new ListResultDto<ProjectDto>(
                ObjectMapper.Map<List<Project>, List<ProjectDto>>(projects)
            );
        }

        public async Task<ProjectDto> GetAsync(string shortName)
        {
            var project = await _projectRepository.GetByShortNameAsync(shortName);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        public async Task<ListResultDto<VersionInfoDto>> GetVersionsAsync(string shortName)
        {
            var project = await _projectRepository.GetByShortNameAsync(shortName);

            var versions = await _versionCache.GetOrAddAsync(
                CacheKeyGenerator.GenerateProjectVersionsCacheKey(project),
                () => GetVersionsAsync(project),
                () => new DistributedCacheEntryOptions
                {
                    //TODO: Configurable?
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12),
                    SlidingExpiration = TimeSpan.FromMinutes(60)
                }
            );

            return new ListResultDto<VersionInfoDto>(
                ObjectMapper.Map<List<VersionInfo>, List<VersionInfoDto>>(versions)
            );
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

        public async Task<LanguageConfig> GetLanguageListAsync(string shortName, string version)
        {
            return await GetLanguageListInternalAsync(shortName, version);
        }

        public async Task<string> GetDefaultLanguageCodeAsync(string shortName, string version)
        {
            var languageList = await GetLanguageListInternalAsync(shortName, version);

            return (languageList.Languages.FirstOrDefault(l => l.IsDefault) ?? languageList.Languages.First()).Code;
        }

        private async Task<LanguageConfig> GetLanguageListInternalAsync(string shortName, string version)
        {
            var project = await _projectRepository.GetByShortNameAsync(shortName);
            var store = _documentSource.Create(project.DocumentStoreType);

            version = GetProjectVersionPrefixIfExist(project) + version;

            async Task<LanguageConfig> GetLanguagesAsync()
            {
                return await store.GetLanguageListAsync(project, version);
            }

            return await LanguageCache.GetOrAddAsync(
                CacheKeyGenerator.GenerateProjectLanguageCacheKey(project),
                GetLanguagesAsync,
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                }
            );
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
