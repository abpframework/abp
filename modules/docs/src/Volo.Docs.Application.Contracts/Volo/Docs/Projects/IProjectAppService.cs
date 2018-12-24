using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Projects
{
    public interface IProjectAppService : IApplicationService
    {
        Task<ListResultDto<ProjectDto>> GetListAsync();
     
        Task<ProjectDto> GetAsync(string shortName);
     
        Task<ProjectDto> CreateAsync(CreateProjectDto input);
     
        Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input);
     
        Task DeleteAsync(Guid id);
        
        Task<ListResultDto<VersionInfoDto>> GetVersionsAsync(string shortName);
    }
}