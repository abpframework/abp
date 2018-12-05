using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Docs.Documents;

namespace Volo.Docs.Projects
{
    public class ProjectAppService : ApplicationService, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDistributedCache<List<VersionInfo>> _distributedCache;
        private readonly IDocumentStoreFactory _documentStoreFactory;

        public ProjectAppService(
            IProjectRepository projectRepository,
            IDistributedCache<List<VersionInfo>> distributedCache,
            IDocumentStoreFactory documentStoreFactory)
        {
            _projectRepository = projectRepository;
            _distributedCache = distributedCache;
            _documentStoreFactory = documentStoreFactory;
        }

        public async Task<ListResultDto<ProjectDto>> GetListAsync()
        {
            var projects = await _projectRepository.GetListAsync();

            return new ListResultDto<ProjectDto>(
                ObjectMapper.Map<List<Project>, List<ProjectDto>>(projects)
            );
        }

        public async Task<ProjectDto> GetByShortNameAsync(string shortName)
        {
            var project = await _projectRepository.GetByShortNameAsync(shortName);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        public async Task<List<VersionInfoDto>> GetVersionsAsync(Guid id)
        {
            //TODO: What if there is no version?

            var project = await _projectRepository.GetAsync(id);
            var documentStore = _documentStoreFactory.Create(project.DocumentStoreType);

            //TODO: Why not use GetOrAddAsync
            var versions = await GetVersionsFromCache(project.ShortName);
            if (versions == null)
            {
                versions = await documentStore.GetVersions(project);
                await SetVersionsToCache(project.ShortName, versions);
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

            return ObjectMapper.Map<List<VersionInfo>, List<VersionInfoDto>>(versions);
        }

        private async Task<List<VersionInfo>> GetVersionsFromCache(string projectShortName)
        {
            return await _distributedCache.GetAsync(projectShortName);
        }

        private async Task SetVersionsToCache(string projectShortName, List<VersionInfo> versions)
        {
            await _distributedCache.SetAsync(
                projectShortName,
                versions,
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(1)
                }
            );
        }
    }
}