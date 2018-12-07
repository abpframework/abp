using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Docs.Projects
{
    [RemoteService]
    [Area("docs")]
    [ControllerName("Project")]
    [Route("api/docs/projects")]
    public class DocsProjectController : AbpController, IProjectAppService
    {
        protected IProjectAppService ProjectAppService { get; }

        public DocsProjectController(IProjectAppService projectAppService)
        {
            ProjectAppService = projectAppService;
        }

        [HttpGet]
        [Route("")]
        public virtual Task<ListResultDto<ProjectDto>> GetListAsync()
        {
            return ProjectAppService.GetListAsync();
        }

        [HttpGet]
        [Route("{shortName}")]
        public virtual Task<ProjectDto> GetAsync(string shortName)
        {
            return ProjectAppService.GetAsync(shortName);
        }

        [HttpGet]
        [Route("{shortName}/versions")]
        public virtual Task<ListResultDto<VersionInfoDto>> GetVersionsAsync(string shortName)
        {
            return ProjectAppService.GetVersionsAsync(shortName);
        }
    }
}
