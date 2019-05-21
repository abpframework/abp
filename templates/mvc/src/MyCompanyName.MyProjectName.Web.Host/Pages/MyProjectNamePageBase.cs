using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using MyCompanyName.MyProjectName.Localization.MyProjectName;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyCompanyName.MyProjectName.Pages
{
    public abstract class MyProjectNamePageBase : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<MyProjectNameResource> L { get; set; }
    }
}
