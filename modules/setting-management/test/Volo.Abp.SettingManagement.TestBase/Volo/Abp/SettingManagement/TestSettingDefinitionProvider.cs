using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement;

public class TestSettingDefinitionProvider : SettingDefinitionProvider
{
    public const string UserOnlySetting = "UserOnlySetting";
    public const string GlobalOnlySetting = "GlobalOnlySetting";

    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new SettingDefinition("MySetting1"));
        context.Add(new SettingDefinition("MySetting2"));
        context.Add(new SettingDefinition("MySetting3", "123"));
        context.Add(new SettingDefinition("MySettingWithoutInherit", isInherited: false));
        context.Add(new SettingDefinition("SettingNotSetInStore", defaultValue: "default-value"));
        context.Add(new SettingDefinition(UserOnlySetting)
            .WithProviders(UserSettingValueProvider.ProviderName));
        context.Add(new SettingDefinition(GlobalOnlySetting)
            .WithProviders(GlobalSettingValueProvider.ProviderName));
    }
}
