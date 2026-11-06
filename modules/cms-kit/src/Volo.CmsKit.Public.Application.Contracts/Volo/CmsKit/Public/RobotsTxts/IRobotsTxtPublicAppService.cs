using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.RobotsTxts;

public interface IRobotsTxtPublicAppService : IApplicationService
{
    Task<RobotsTxtPublicDto> GetAsync();
}
