using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Guids;
using Volo.Docs.Documents;
using Volo.Docs.Documents.FullSearch.Elastic;
using Volo.Docs.Localization;
using Volo.Docs.Projects;

namespace Volo.Docs.Admin.Projects
{
    [Authorize(DocsAdminPermissions.Projects.Default)]
    public class ProjectAdminAppService : ApplicationService, IProjectAdminAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentFullSearch _elasticSearchService;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public ProjectAdminAppService(
            IProjectRepository projectRepository,
            IDocumentRepository documentRepository,
            IDocumentFullSearch elasticSearchService,
            IGuidGenerator guidGenerator,
            IBackgroundJobManager backgroundJobManager)
        {
            ObjectMapperContext = typeof(DocsAdminApplicationModule);
            LocalizationResource = typeof(DocsResource);

            _projectRepository = projectRepository;
            _documentRepository = documentRepository;
            _elasticSearchService = elasticSearchService;
            _guidGenerator = guidGenerator;
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task<PagedResultDto<ProjectDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var projects = await _projectRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount);

            var totalCount = await _projectRepository.GetCountAsync();

            return new PagedResultDto<ProjectDto>(
                totalCount,
                ObjectMapper.Map<List<Project>, List<ProjectDto>>(projects)
                );
        }

        public async Task<ProjectDto> GetAsync(Guid id)
        {
            var project = await _projectRepository.GetAsync(id);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(DocsAdminPermissions.Projects.Create)]
        public async Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            if (await _projectRepository.ShortNameExistsAsync(input.ShortName))
            {
                throw new ProjectShortNameAlreadyExistsException(input.ShortName);
            }

            var project = new Project(_guidGenerator.Create(),
                input.Name,
                input.ShortName,
                input.DocumentStoreType,
                input.Format,
                input.DefaultDocumentName,
                input.NavigationDocumentName,
                input.ParametersDocumentName
            )
            {
                MinimumVersion = input.MinimumVersion,
                MainWebsiteUrl = input.MainWebsiteUrl,
                LatestVersionBranchName = input.LatestVersionBranchName
            };

            foreach (var extraProperty in input.ExtraProperties)
            {
                project.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
            }

            project = await _projectRepository.InsertAsync(project);

            return ObjectMapper.Map<Project, ProjectDto>(project);
        }

        [Authorize(DocsAdminPermissions.Projects.Update)]
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

        [Authorize(DocsAdminPermissions.Projects.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _projectRepository.DeleteAsync(id);
        }

        public async Task ReindexAsync(ReindexInput input)
        {
            _elasticSearchService.ValidateElasticSearchEnabled();
            if (!_backgroundJobManager.IsAvailable())
            {
                throw new UserFriendlyException("Background Job Manager is not available!");
            }

            await _backgroundJobManager.EnqueueAsync(new ProjectIndexBackgroundWorkerArgs(input.ProjectId));
        }

        //private async Task ReindexProjectAsync(Guid projectId)
        //{
        //    var project = await _projectRepository.FindAsync(projectId);
        //    if (project == null)
        //    {
        //        throw new Exception("Cannot find the project with the Id " + projectId);
        //    }

        //    var docs = await _documentRepository.GetListByProjectId(project.Id);
        //    await _elasticSearchService.DeleteAllByProjectIdAsync(project.Id);

        //    foreach (var doc in docs)
        //    {
        //        if (doc.FileName == project.NavigationDocumentName)
        //        {
        //            continue;
        //        }

        //        if (doc.FileName == project.ParametersDocumentName)
        //        {
        //            continue;
        //        }

        //        await _elasticSearchService.AddOrUpdateAsync(doc);
        //    }
        //}

        public async Task ReindexAllAsync()
        {
            _elasticSearchService.ValidateElasticSearchEnabled();
            if (!_backgroundJobManager.IsAvailable())
            {
                throw new UserFriendlyException("Background Job Manager is not available!");
            }

            var projects = await _projectRepository.GetListAsync();
            foreach (var project in projects)
            {
                await _backgroundJobManager.EnqueueAsync(new ProjectIndexBackgroundWorkerArgs(project.Id));

                //await ReindexProjectAsync(project.Id);
            }
        }
    }
}
