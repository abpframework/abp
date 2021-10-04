using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MyCompanyName.MyProjectName
{
    public abstract class MyProjectNameController : AbpControllerBase
    {
        protected MyProjectNameController()
        {
            LocalizationResource = typeof(MyProjectNameResource);
        }
    }
}
