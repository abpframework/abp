using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.GlobalFeatures;

namespace Volo.Abp.AspNetCore.Mvc.GlobalFeatures;

[RequiresGlobalFeature(AbpAspNetCoreMvcTestFeature1.Name)]
public class EnabledGlobalFeatureTestPage : PageModel
{
    public void OnGet()
    {

    }
}
