using Volo.Abp.Settings;

namespace Volo.Abp.Localization
{
    public class LocalizationSettingProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(LocalizationSettingNames.DefaultLanguage, "en", isVisibleToClients: true)
            );
        }
    }
}