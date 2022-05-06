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
        
        group.AddFeature(
            "MaxNumber",
            "",
            L("Max number"),
            new FixedLocalizableString("Test feature max number"),
            new FreeTextStringValueType());

        settingEnableFeature.CreateChild(
            SettingManagementFeatures.AllowTenantsToChangeEmailSettings,
            "false",
            L("Feature:AllowTenantsToChangeEmailSettings"),
            L("Feature:AllowTenantsToChangeEmailSettingsDescription"),
            new ToggleStringValueType(),
            isAvailableToHost: false);
        
        settingEnableFeature.CreateChild(
            "MinNumber",
            "",
            L("Min number"),
            new FixedLocalizableString("Test feature min number"),
            new FreeTextStringValueType(),
            isAvailableToHost: false);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSettingManagementResource>(name);
    }
}
