using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MyCompanyName.MyProjectName.Controllers
{
    public abstract class MyProjectNameController : AbpController
    {
        protected MyProjectNameController()
        {
            LocalizationResource = typeof(MyProjectNameResource);
        }
    }
}