using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MyCompanyName.MyProjectName
{
    public class MyProjectNameControllerBase : AbpController
    {
        public MyProjectNameControllerBase()
        {
            LocalizationResource = typeof(MyProjectNameResource);
        }
    }
}
