using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.GlobalResources;

public interface IGlobalResourceAdminAppService : IApplicationService
{
    Task<GlobalResourcesDto> GetAsync();

    Task SetGlobalResourcesAsync(GlobalResourcesUpdateDto input);
}