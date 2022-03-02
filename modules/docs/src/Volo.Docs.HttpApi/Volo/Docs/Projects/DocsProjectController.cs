using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Docs.Documents;

namespace Volo.Docs.Projects
{
    [RemoteService(Name = DocsRemoteServiceConsts.RemoteServiceName)]
    [Area(DocsRemoteServiceConsts.ModuleName)]
    [ControllerName("Project")]
    [Route("api/docs/projects")]
    public class DocsProjectController : AbpControllerBase, IProjectAppService
    {
        protected IProjectAppService ProjectAppService { get; }

        public DocsProjectController(IProjectAppService projectAppService)
        {
            ProjectAppService = projectAppService;
        }

        [HttpGet]
        [Route("")]
        public virtual async Task<ListResultDto<ProjectDto>> GetListAsync()
        {
            var projects = await ProjectAppService.GetListAsync();
            projects.Items = projects.Items.Select(project =>
            {
                project.ExtraProperties = new Dictionary<string, object>();
                return project;
            }).ToList();
            
            return projects;
        }

        [HttpGet]
        [Route("{shortName}")]
        public virtual async Task<ProjectDto> GetAsync(string shortName)
        {
            var project = await ProjectAppService.GetAsync(shortName);
            project.ExtraProperties = new Dictionary<string, object>();
            return project;
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
