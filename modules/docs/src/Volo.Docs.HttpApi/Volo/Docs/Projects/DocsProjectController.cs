using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Docs.Documents;

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
        [Route("{shortName}/defaultLanguage")]
        public Task<string> GetDefaultLanguageCodeAsync(string shortName,string version)
        {
            return ProjectAppService.GetDefaultLanguageCodeAsync(shortName, version);
        }

        [HttpGet]
        [Route("{shortName}/versions")]
        public virtual Task<ListResultDto<VersionInfoDto>> GetVersionsAsync(string shortName)
        {
            return ProjectAppService.GetVersionsAsync(shortName);
        }

        [HttpGet]
        [Route("{shortName}/{version}/languageList")]
        public Task<LanguageConfig> GetLanguageListAsync(string shortName, string version)
        {
            return ProjectAppService.GetLanguageListAsync(shortName, version);
        }
    }
}
