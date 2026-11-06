using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.RobotsTxts;

public interface IRobotsTxtAdminAppService : IApplicationService
{
    Task<RobotsTxtDto> GetAsync();

    Task UpdateAsync(UpdateRobotsTxtDto input);

    Task ResetAsync();
}
