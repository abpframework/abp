using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyCompanyName.MyProjectName.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class MyProjectNamePageModel : AbpPageModel
    {
        protected MyProjectNamePageModel()
        {
            LocalizationResourceType = typeof(MyProjectNameResource);
            ObjectMapperContext = typeof(MyProjectNameWebModule);
        }
    }
}