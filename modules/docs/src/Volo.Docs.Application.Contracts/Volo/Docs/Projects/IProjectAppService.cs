using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Docs.Documents;

namespace Volo.Docs.Projects
{
    public interface IProjectAppService : IApplicationService
    {
        Task<ListResultDto<ProjectDto>> GetListAsync();
     
        Task<ProjectDto> GetByShortNameAsync(string shortName);
        
        Task<List<VersionInfoDto>> GetVersionsAsync(Guid projectId);
    }
}