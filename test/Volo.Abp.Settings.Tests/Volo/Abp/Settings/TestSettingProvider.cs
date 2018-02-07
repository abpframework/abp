namespace Volo.Abp.Settings
{
    public class TestSettingProvider : SettingProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(new SettingDefinition("MySetting1"));
            context.Add(new SettingDefinition("MySetting2"));
            context.Add(new SettingDefinition("SettingNotSetInStore", defaultValue: "default-value"));
        }
    }
}