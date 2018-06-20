using Microsoft.AspNetCore.Mvc.Localization;
using MyCompanyName.MyProjectName.Localization.MyProjectName;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyCompanyName.MyProjectName.Pages
{
    public abstract class MyProjectNamePageModelBase : AbpPageModel
    {
        public IHtmlLocalizer<MyProjectNameResource> L { get; set; }
    }
}