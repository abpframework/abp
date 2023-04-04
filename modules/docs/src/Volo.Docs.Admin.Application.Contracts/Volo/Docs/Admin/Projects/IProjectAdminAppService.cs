using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Admin.Projects
{
    public interface IProjectAdminAppService : IApplicationService
    {
        Task<PagedResultDto<ProjectDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<ProjectDto> GetAsync(Guid id);

        Task<ProjectDto> CreateAsync(CreateProjectDto input);

        Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input);

        Task DeleteAsync(Guid id);

        Task ReindexAsync(ReindexInput input);

        Task ReindexAllAsync();
        Task<List<ProjectWithoutDetailsDto>> GetListWithoutDetailsAsync();
    }
}
