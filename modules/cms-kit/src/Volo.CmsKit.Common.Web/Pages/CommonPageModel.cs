using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Web.Pages;

public abstract class CommonPageModel : AbpPageModel
{
    protected CommonPageModel()
    {
        LocalizationResourceType = typeof(CmsKitResource);
    }
}
