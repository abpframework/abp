using Bench.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Bench.Web.Pages;

public abstract class BenchPageModel : AbpPageModel
{
    protected BenchPageModel()
    {
        LocalizationResourceType = typeof(BenchResource);
    }
}
