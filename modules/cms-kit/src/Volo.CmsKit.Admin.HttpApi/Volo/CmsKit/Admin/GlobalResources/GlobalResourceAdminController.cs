using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.GlobalResources;

[RequiresGlobalFeature(typeof(GlobalResourcesFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitAdminRemoteServiceConsts.ModuleName)]
[Authorize(CmsKitAdminPermissions.Menus.Default)]
[Route("api/cms-kit-admin/global-resources")]
public class GlobalResourceAdminController: CmsKitAdminController, IGlobalResourceAdminAppService
{
    private readonly IGlobalResourceAdminAppService _globalResourceAdminAppService;

    public GlobalResourceAdminController(IGlobalResourceAdminAppService globalResourceAdminAppService)
    {
        _globalResourceAdminAppService = globalResourceAdminAppService;
    }
    
    [HttpGet]
    public Task<GlobalResourcesDto> GetAsync()
    {
        return _globalResourceAdminAppService.GetAsync();
    }

    [HttpPost]
    public Task SetGlobalResourcesAsync(GlobalResourcesUpdateDto input)
    {
        return _globalResourceAdminAppService.SetGlobalResourcesAsync(input);
    }
}