using Volo.Abp.Settings;

namespace Volo.Abp.Account.Web.Settings
{
    public class AccountSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(AccountSettingNames.IsSelfRegistrationEnabled, "true")
            );
        }
    }
}