using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Docs.Admin.Projects
{
    [RemoteService]
    [Area("docs")]
    [ControllerName("Project")]
    [Route("api/docs/admin/projects")]
    public class DocsAdminProjectController : AbpController, IProjectAdminAppService
    {
        protected IProjectAdminAppService ProjectAppService { get; }

        public DocsAdminProjectController(IProjectAdminAppService projectAppService)
        {
            ProjectAppService = projectAppService;
        }

        public Task<PagedResultDto<ProjectDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return ProjectAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<ProjectDto> GetAsync(Guid id)
        {
            return ProjectAppService.GetAsync(id);
        }

        public Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            return ProjectAppService.CreateAsync(input);
        }

        public Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input)
        {
            return ProjectAppService.UpdateAsync(id, input);
        }

        public Task DeleteAsync(Guid id)
        {
            return ProjectAppService.DeleteAsync(id);
        }
    }
}
