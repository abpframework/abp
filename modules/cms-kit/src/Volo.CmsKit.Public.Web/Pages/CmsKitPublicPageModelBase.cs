using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Public.Web.Pages;

public abstract class CmsKitPublicPageModelBase : AbpPageModel
{
    protected CmsKitPublicPageModelBase()
    {
        LocalizationResourceType = typeof(CmsKitResource);
        ObjectMapperContext = typeof(CmsKitPublicWebModule);
    }
}
