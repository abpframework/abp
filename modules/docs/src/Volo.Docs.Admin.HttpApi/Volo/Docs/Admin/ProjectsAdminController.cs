﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Docs.Admin.Projects;

namespace Volo.Docs.Admin
{
    [RemoteService(Name = DocsAdminRemoteServiceConsts.RemoteServiceName)]
    [Area(DocsAdminRemoteServiceConsts.ModuleName)]
    [ControllerName("ProjectsAdmin")]
    [Route("api/docs/admin/projects")]
    public class ProjectsAdminController : AbpControllerBase, IProjectAdminAppService
    {
        private readonly IProjectAdminAppService _projectAppService;

        public ProjectsAdminController(IProjectAdminAppService projectAdminAppService)
        {
            _projectAppService = projectAdminAppService;
        }

        [HttpGet]
        [Route("")]
        public Task<PagedResultDto<ProjectDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _projectAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<ProjectDto> GetAsync(Guid id)
        {
            return _projectAppService.GetAsync(id);
        }

        [HttpPost]
        public Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            return _projectAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input)
        {
            return _projectAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public Task DeleteAsync(Guid id)
        {
            return _projectAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Route("ReindexAll")]
        public Task ReindexAllAsync()
        {
            return _projectAppService.ReindexAllAsync();
        }

        [HttpGet]
        [Route("GetListProjectWithoutDetailsAsync")]
        public Task<List<ProjectWithoutDetailsDto>> GetListWithoutDetailsAsync()
        {
            return _projectAppService.GetListWithoutDetailsAsync();
        }

        [HttpPost]
        [Route("Reindex")]
        public Task ReindexAsync(ReindexInput input)
        {
            return _projectAppService.ReindexAsync(input);
        }
    }
}
