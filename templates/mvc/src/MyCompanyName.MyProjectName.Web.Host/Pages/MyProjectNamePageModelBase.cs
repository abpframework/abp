using MyCompanyName.MyProjectName.Localization.MyProjectName;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyCompanyName.MyProjectName.Pages
{
    public abstract class MyProjectNamePageModelBase : AbpPageModel
    {
        protected MyProjectNamePageModelBase()
        {
            LocalizationResourceType = typeof(MyProjectNameResource);
        }
    }
}