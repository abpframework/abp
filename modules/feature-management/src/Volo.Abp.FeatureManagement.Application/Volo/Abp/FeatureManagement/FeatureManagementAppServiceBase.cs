using Volo.Abp.Application.Services;
using Volo.Abp.FeatureManagement.Localization;

namespace Volo.Abp.FeatureManagement
{
    public abstract class FeatureManagementAppServiceBase : ApplicationService
    {
        protected FeatureManagementAppServiceBase()
        {
            ObjectMapperContext = typeof(AbpFeatureManagementApplicationModule);
            LocalizationResource = typeof(AbpFeatureManagementResource);
        }
    }
}