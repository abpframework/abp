using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.SettingManagement;

public class SettingManagementFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(SettingManagementFeatures.GroupName,
            L("Feature:SettingManagementGroup"));

        var settingEnableFeature = group.AddFeature(
            SettingManagementFeatures.Enable,
            "true",
            L("Feature:SettingManagementEnable"),
            L("Feature:SettingManagementEnableDescription"),
            new ToggleStringValueType());

        settingEnableFeature.CreateChild(
            SettingManagementFeatures.AllowTenantsToChangeEmailSettings,
            "false",
            L("Feature:AllowTenantsToChangeEmailSettings"),
            L("AllowTenantsToChangeEmailSettingsDescription"),
            new ToggleStringValueType(),
            isAvailableToHost: false);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSettingManagementResource>(name);
    }
}
