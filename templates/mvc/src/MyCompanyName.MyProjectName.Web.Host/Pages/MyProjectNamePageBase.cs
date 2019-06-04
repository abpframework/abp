using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyCompanyName.MyProjectName.Web.Pages
{
    public abstract class MyProjectNamePageBase : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<MyProjectNameResource> L { get; set; }
    }
}
