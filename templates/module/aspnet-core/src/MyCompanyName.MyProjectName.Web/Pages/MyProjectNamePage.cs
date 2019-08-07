using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyCompanyName.MyProjectName.Web.Pages
{
    /* Inherit your UI Pages from this class. To do that, add this line to your Pages (.cshtml files under the Page folder):
     * @inherits MyCompanyName.MyProjectName.Web.Pages.MyProjectNamePage
     */
    public abstract class MyProjectNamePage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<MyProjectNameResource> L { get; set; }
    }
}
