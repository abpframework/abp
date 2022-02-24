using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.GlobalResources;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.GlobalResources;

[RequiresGlobalFeature(typeof(GlobalResourcesFeature))]
[Authorize(CmsKitAdminPermissions.GlobalResources.Default)]
public class GlobalResourceAdminAppService : ApplicationService, IGlobalResourceAdminAppService
{
    public GlobalResourceManager GlobalResourceManager { get; }

    public GlobalResourceAdminAppService(GlobalResourceManager globalResourceManager)
    {
        GlobalResourceManager = globalResourceManager;
    }
    
    public async Task<GlobalResourcesDto> GetAsync()
    {
        return new GlobalResourcesDto {
            StyleContent = (await GlobalResourceManager.GetGlobalStyleAsync()).Value,
            ScriptContent = (await GlobalResourceManager.GetGlobalScriptAsync()).Value
        };
    }

    public async Task SetGlobalStyleAsync(GlobalResourceUpdateDto input)
    {
        await GlobalResourceManager.SetGlobalStyleAsync(input.Value);
    }

    public async Task SetGlobalScriptAsync(GlobalResourceUpdateDto input)
    {
        await GlobalResourceManager.SetGlobalScriptAsync(input.Value);
    }
}