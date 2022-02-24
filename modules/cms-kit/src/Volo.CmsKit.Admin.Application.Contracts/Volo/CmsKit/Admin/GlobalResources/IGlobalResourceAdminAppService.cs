using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.GlobalResources;

public interface IGlobalResourceAdminAppService : IApplicationService
{
    Task<GlobalResourcesDto> GetAsync();

    Task SetGlobalStyleAsync(GlobalResourceUpdateDto input);
    
    Task SetGlobalScriptAsync(GlobalResourceUpdateDto input);
}