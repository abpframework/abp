using DashboardDemo.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace DashboardDemo.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class DashboardDemoPageModel : AbpPageModel
    {
        protected DashboardDemoPageModel()
        {
            LocalizationResourceType = typeof(DashboardDemoResource);
        }
    }
}