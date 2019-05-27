using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.Application.Services;

namespace MyCompanyName.MyProjectName
{
    public abstract class MyProjectNameAppServiceBase : ApplicationService
    {
        protected MyProjectNameAppServiceBase()
        {
            LocalizationResource = typeof(MyProjectNameResource);
        }
    }
}
