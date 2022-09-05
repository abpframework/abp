using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Public.GlobalResources;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.GlobalResources.Style;

public class GlobalStyleViewComponent : AbpViewComponent
{
    protected IGlobalResourcePublicAppService GlobalResourcePublicAppService { get; }

    public GlobalStyleViewComponent(IGlobalResourcePublicAppService globalResourcePublicAppService)
    {
        GlobalResourcePublicAppService = globalResourcePublicAppService;
    }

    [BindProperty(SupportsGet = true)]
    public DateTime? LastModificationTime { get; set; }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var lastModificationTime = (await GlobalResourcePublicAppService.GetGlobalStyleAsync())?.LastModificationTime;
        var lastModificationTimeTimestamp = (long)(lastModificationTime.HasValue ? lastModificationTime.Value.Subtract(DateTime.UnixEpoch).TotalSeconds : 0);

        return View("~/Pages/CmsKit/Shared/Components/GlobalResources/Style/Default.cshtml",
            new GlobalStyleModel()
            {
                LastModificationTimeTimestamp = lastModificationTimeTimestamp
            });
    }

}