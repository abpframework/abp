using Volo.CmsKit.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.CmsKit.Pages;

public abstract class CmsKitPageModel : AbpPageModel
{
    protected CmsKitPageModel()
    {
        LocalizationResourceType = typeof(CmsKitResource);
    }
}
