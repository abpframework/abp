using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Admin.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class CmsKitAdminPageModel : AbpPageModel
{
    protected CmsKitAdminPageModel()
    {
        LocalizationResourceType = typeof(CmsKitResource);
        ObjectMapperContext = typeof(CmsKitAdminWebModule);
    }
}
