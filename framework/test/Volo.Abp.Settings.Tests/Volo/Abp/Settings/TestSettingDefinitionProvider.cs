namespace Volo.Abp.Settings;

public class TestSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(TestSettingNames.TestSettingWithoutDefaultValue),
            new SettingDefinition(TestSettingNames.TestSettingWithDefaultValue, "default-value"),
            new SettingDefinition(TestSettingNames.TestSettingEncrypted, isEncrypted: true)
        );
    }
}
