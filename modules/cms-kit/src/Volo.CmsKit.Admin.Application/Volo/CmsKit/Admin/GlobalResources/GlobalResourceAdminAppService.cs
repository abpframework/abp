using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.GlobalResources;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.GlobalResources;

[RequiresFeature(CmsKitFeatures.GlobalResourceEnable)]
[RequiresGlobalFeature(typeof(GlobalResourcesFeature))]
[Authorize(CmsKitAdminPermissions.GlobalResources.Default)]
public class GlobalResourceAdminAppService : ApplicationService, IGlobalResourceAdminAppService
{
    public GlobalResourceManager GlobalResourceManager { get; }

    public GlobalResourceAdminAppService(GlobalResourceManager globalResourceManager)
    {
        GlobalResourceManager = globalResourceManager;
    }

    public virtual async Task<GlobalResourcesDto> GetAsync()
    {
        return new GlobalResourcesDto
        {
            StyleContent = (await GlobalResourceManager.GetGlobalStyleAsync()).Value,
            ScriptContent = (await GlobalResourceManager.GetGlobalScriptAsync()).Value
        };
    }

    public virtual async Task SetGlobalResourcesAsync(GlobalResourcesUpdateDto input)
    {
        await GlobalResourceManager.SetGlobalStyleAsync(input.Style);
        await GlobalResourceManager.SetGlobalScriptAsync(input.Script);
    }
}