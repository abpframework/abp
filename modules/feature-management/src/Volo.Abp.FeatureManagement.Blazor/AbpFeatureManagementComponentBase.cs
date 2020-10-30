using Volo.Abp.AspNetCore.Components;
using Volo.Abp.FeatureManagement.Localization;

namespace Volo.Abp.FeatureManagement.Blazor
{
    public class AbpFeatureManagementComponentBase : AbpComponentBase
    {
        public AbpFeatureManagementComponentBase()
        {
            LocalizationResource = typeof(AbpFeatureManagementResource);
        }
    }
}