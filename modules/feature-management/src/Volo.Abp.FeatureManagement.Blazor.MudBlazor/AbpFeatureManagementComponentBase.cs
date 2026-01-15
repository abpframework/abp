using Volo.Abp.AspNetCore.Components;
using Volo.Abp.FeatureManagement.Localization;

namespace Volo.Abp.FeatureManagement.Blazor.MudBlazor;

public abstract class AbpFeatureManagementComponentBase : AbpComponentBase
{
    protected AbpFeatureManagementComponentBase()
    {
        LocalizationResource = typeof(AbpFeatureManagementResource);
    }
}
