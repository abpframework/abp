using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Public.GlobalResources;

namespace Volo.CmsKit.Public.Web.Controllers;

[Route("global-resources")]
public class CmsKitPublicGlobalResourcesController: AbpController
{
    private readonly IGlobalResourcePublicAppService _globalResourcePublicAppService;

    public CmsKitPublicGlobalResourcesController(IGlobalResourcePublicAppService globalResourcePublicAppService)
    {
        _globalResourcePublicAppService = globalResourcePublicAppService;
    }
    
    [HttpGet]
    [Route("style")]
    public async Task<IActionResult> GetGlobalStyleAsync(string entityType, string entityId)
    {
        var style = await _globalResourcePublicAppService.GetGlobalStyleAsync();
        
        var contents = Encoding.ASCII.GetBytes(style.Value);
        
        return new FileContentResult(contents, "text/css");
    }
    
    [HttpGet]
    [Route("script")]
    public async Task<IActionResult> GetGlobalScriptAsync(string entityType, string entityId)
    {
        var script = await _globalResourcePublicAppService.GetGlobalScriptAsync();
        
        var contents = Encoding.ASCII.GetBytes(script.Value);
        
        return new FileContentResult(contents, "application/javascript");
    }
}