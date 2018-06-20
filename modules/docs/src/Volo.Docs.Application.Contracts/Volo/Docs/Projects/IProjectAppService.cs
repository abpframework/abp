using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Projects
{
    public interface IProjectAppService : IApplicationService
    {
        Task<ListResultDto<ProjectDto>> GetListAsync();
    }
}