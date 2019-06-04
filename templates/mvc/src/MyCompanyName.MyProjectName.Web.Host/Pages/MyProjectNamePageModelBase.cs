using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyCompanyName.MyProjectName.Web.Pages
{
    public abstract class MyProjectNamePageModelBase : AbpPageModel
    {
        protected MyProjectNamePageModelBase()
        {
            LocalizationResourceType = typeof(MyProjectNameResource);
        }
    }
}