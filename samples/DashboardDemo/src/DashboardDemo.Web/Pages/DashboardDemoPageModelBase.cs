using DashboardDemo.Localization.DashboardDemo;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace DashboardDemo.Pages
{
    public abstract class DashboardDemoPageModelBase : AbpPageModel
    {
        protected DashboardDemoPageModelBase()
        {
            LocalizationResourceType = typeof(DashboardDemoResource);
        }
    }
}