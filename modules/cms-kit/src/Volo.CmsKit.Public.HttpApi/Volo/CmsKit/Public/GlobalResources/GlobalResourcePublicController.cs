using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.GlobalResources;

[RequiresGlobalFeature(typeof(GlobalResourcesFeature))]
[RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitPublicRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-public/global-resources")]
public class GlobalResourcePublicController: CmsKitPublicControllerBase, IGlobalResourcePublicAppService
{
    private readonly IGlobalResourcePublicAppService _globalResourcePublicAppService;

    public GlobalResourcePublicController(IGlobalResourcePublicAppService globalResourcePublicAppService)
    {
        _globalResourcePublicAppService = globalResourcePublicAppService;
    }
    
    [HttpGet]
    [Route("script")]
    public Task<GlobalResourceDto> GetGlobalScriptAsync()
    {
        return _globalResourcePublicAppService.GetGlobalScriptAsync();
    }
    
    [HttpGet]
    [Route("style")]
    public Task<GlobalResourceDto> GetGlobalStyleAsync()
    {
        return _globalResourcePublicAppService.GetGlobalStyleAsync();
    }
}