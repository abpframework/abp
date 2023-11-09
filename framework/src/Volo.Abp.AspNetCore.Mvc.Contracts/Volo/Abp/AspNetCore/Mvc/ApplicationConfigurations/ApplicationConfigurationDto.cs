using System;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using Volo.Abp.Data;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Serializable]
public class ApplicationConfigurationDto : IHasExtraProperties
{
    public ApplicationLocalizationConfigurationDto Localization { get; set; }

    public ApplicationAuthConfigurationDto Auth { get; set; }

    public ApplicationSettingConfigurationDto Setting { get; set; }

    public CurrentUserDto CurrentUser { get; set; }

    public ApplicationFeatureConfigurationDto Features { get; set; }

    public ApplicationGlobalFeatureConfigurationDto GlobalFeatures { get; set; }

    public MultiTenancyInfoDto MultiTenancy { get; set; }

    public CurrentTenantDto CurrentTenant { get; set; }

    public TimingDto Timing { get; set; }

    public ClockDto Clock { get; set; }

    public ObjectExtensionsDto ObjectExtensions { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; }

    public ApplicationConfigurationDto()
    {
        Localization = new ApplicationLocalizationConfigurationDto();
        Auth = new ApplicationAuthConfigurationDto();
        Setting = new ApplicationSettingConfigurationDto();
        CurrentUser = new CurrentUserDto();
        Features = new ApplicationFeatureConfigurationDto();
        GlobalFeatures = new ApplicationGlobalFeatureConfigurationDto();
        MultiTenancy = new MultiTenancyInfoDto();
        CurrentTenant = new CurrentTenantDto();
        Timing = new TimingDto();
        Clock = new ClockDto();
        ObjectExtensions = new ObjectExtensionsDto();
        ExtraProperties = new ExtraPropertyDictionary();
    }
}
