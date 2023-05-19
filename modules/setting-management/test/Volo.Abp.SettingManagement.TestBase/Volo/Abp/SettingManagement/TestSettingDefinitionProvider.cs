using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement;

public class TestSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new SettingDefinition("MySetting1"));
        context.Add(new SettingDefinition("MySetting2"));
        context.Add(new SettingDefinition("MySetting3", "123"));
        context.Add(new SettingDefinition("MySettingWithoutInherit", isInherited: false));
        context.Add(new SettingDefinition("SettingNotSetInStore", defaultValue: "default-value"));


    }
}
