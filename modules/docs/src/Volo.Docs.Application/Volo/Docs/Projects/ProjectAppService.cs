using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Guids;
using Volo.Docs.Documents;

namespace Volo.Docs.Projects
{
    public class ProjectAppService : ApplicationService, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDistributedCache<List<VersionInfo>> _versionCache;
        private readonly IDocumentStoreFactory _documentStoreFactory;
        private readonly IGuidGenerator _guidGenerator;

        public ProjectAppService(
            IProjectRepository projectRepository,
            IDistributedCache<List<VersionInfo>> versionCache,
            IDocumentStoreFactory documentStoreFactory, IGuidGenerator guidGenerator)
        {
            _projectRepository = projectRepository;
            _versionCache = versionCache;
            _documentStoreFactory = documentStoreFactory;
            _guidGenerator = guidGenerator;
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

        public async Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            var project = new Project(_guidGenerator.Create(),
                input.Name,
                input.ShortName,
                input.DocumentStoreType,
                input.Format,
                input.DefaultDocumentName,
                input.NavigationDocumentName
            )
            {
                MinimumVersion = input.MinimumVersion,
                MainWebsiteUrl = input.MainWebsiteUrl,
                LatestVersionBranchName = input.LatestVersionBranchName
            };

            foreach (var extraProperty in input.ExtraProperties)
            {
                project.ExtraProperties.Add(extraProperty.Key,extraProperty.Value);
            }

            project = await _projectRepository.InsertAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        public async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input)
        {
            var project = await _projectRepository.GetAsync(id);

            project.SetName(input.Name);
            project.SetFormat(input.Format);
            project.SetNavigationDocumentName(input.NavigationDocumentName);
            project.SetDefaultDocumentName(input.DefaultDocumentName);

            project.MinimumVersion = input.MinimumVersion;
            project.MainWebsiteUrl = input.MainWebsiteUrl;
            project.LatestVersionBranchName = input.LatestVersionBranchName;

            foreach (var extraProperty in input.ExtraProperties)
            {
                project.ExtraProperties[extraProperty.Key] = extraProperty.Value;
            }

            project = await _projectRepository.UpdateAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _projectRepository.DeleteAsync(id);
        }

        public async Task<ListResultDto<VersionInfoDto>> GetVersionsAsync(string shortName)
        {
            var project = await _projectRepository.GetByShortNameAsync(shortName);

            var versions = await _versionCache.GetOrAddAsync(
                project.ShortName,
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
            var store = _documentStoreFactory.Create(project.DocumentStoreType);
            var versions = await store.GetVersions(project);

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

            if (versions.Any() && !string.IsNullOrEmpty(project.LatestVersionBranchName))
            {
                versions.First().Name = project.LatestVersionBranchName;
            }

            return versions;
        }
    }
}