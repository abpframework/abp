using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MyCompanyName.MyProjectName.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class MyProjectNameController : AbpController
    {
        protected MyProjectNameController()
        {
            LocalizationResource = typeof(MyProjectNameResource);
        }
    }
}