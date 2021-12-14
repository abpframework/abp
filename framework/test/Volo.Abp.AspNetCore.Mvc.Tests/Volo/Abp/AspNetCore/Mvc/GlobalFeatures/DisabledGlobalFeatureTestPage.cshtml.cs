using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.GlobalFeatures;

namespace Volo.Abp.AspNetCore.Mvc.GlobalFeatures;

[RequiresGlobalFeature("Not-Enabled-Feature")]
public class DisabledGlobalFeatureTestPage : PageModel
{
    public void OnGet()
    {

    }
}
