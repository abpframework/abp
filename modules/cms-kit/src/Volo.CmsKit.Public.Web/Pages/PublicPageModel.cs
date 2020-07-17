using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class PublicPageModel : AbpPageModel
    {
        protected PublicPageModel()
        {
            LocalizationResourceType = typeof(CmsKitResource);
            ObjectMapperContext = typeof(CmsKitPublicWebModule);
        }
    }
}