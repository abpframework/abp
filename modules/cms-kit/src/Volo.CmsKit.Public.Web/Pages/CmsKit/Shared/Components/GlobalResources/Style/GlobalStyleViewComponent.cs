using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Features;
using Volo.CmsKit.Features;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.GlobalResources.Style;

public class GlobalStyleViewComponent : AbpViewComponent
{
    protected IFeatureChecker FeatureChecker { get; }

    public GlobalStyleViewComponent(IFeatureChecker featureChecker)
    {
        FeatureChecker = featureChecker;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (!await FeatureChecker.IsEnabledAsync(CmsKitFeatures.GlobalResourceEnable))
        {
            return Content(string.Empty);
        }

        return View("~/Pages/CmsKit/Shared/Components/GlobalResources/Style/Default.cshtml");
    }
}