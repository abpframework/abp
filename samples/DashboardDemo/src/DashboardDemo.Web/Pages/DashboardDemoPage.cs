using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using DashboardDemo.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace DashboardDemo.Web.Pages
{
    /* Inherit your UI Pages from this class. To do that, add this line to your Pages (.cshtml files under the Page folder):
     * @inherits DashboardDemo.Web.Pages.DashboardDemoPage
     */
    public abstract class DashboardDemoPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<DashboardDemoResource> L { get; set; }
    }
}
