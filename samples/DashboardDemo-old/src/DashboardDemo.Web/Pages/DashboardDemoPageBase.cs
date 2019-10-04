using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using DashboardDemo.Localization.DashboardDemo;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace DashboardDemo.Pages
{
    public abstract class DashboardDemoPageBase : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<DashboardDemoResource> L { get; set; }
    }
}
