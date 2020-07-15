using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Admin.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class AdminPageModel : AbpPageModel
    {
        protected AdminPageModel()
        {
            LocalizationResourceType = typeof(CmsKitResource);
            ObjectMapperContext = typeof(CmsKitAdminWebModule);
        }
    }
}