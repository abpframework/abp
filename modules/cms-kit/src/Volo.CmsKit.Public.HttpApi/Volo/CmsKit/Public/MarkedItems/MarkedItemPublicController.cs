using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.MarkedItems;

[RequiresFeature(CmsKitFeatures.MarkedItemEnable)]
[RequiresGlobalFeature(typeof(MarkedItemsFeature))]
[RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitPublicRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-public/marked-items")]
public class MarkedItemPublicController : CmsKitPublicControllerBase, IMarkedItemPublicAppService
{
    protected IMarkedItemPublicAppService MarkedItemPublicAppService { get; }

    public MarkedItemPublicController(IMarkedItemPublicAppService markedItemPublicAppService)
    {
        MarkedItemPublicAppService = markedItemPublicAppService;
    }

    [HttpGet]
    [Route("{entityType}/{entityId}")]
    public virtual Task<MarkedItemWithToggleDto> GetForUserAsync(string entityType, string entityId)
    {
        return MarkedItemPublicAppService.GetForUserAsync(entityType, entityId);
    }

    [HttpPut]
    [Route("{entityType}/{entityId}")]
    public Task<bool> ToggleAsync(string entityType, string entityId)
    {
        return MarkedItemPublicAppService.ToggleAsync(entityType, entityId);
    }
}
