using Volo.Abp.Account.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Volo.Abp.Account.Settings
{
    public class AccountSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    AccountSettingNames.IsSelfRegistrationEnabled, 
                    "true", 
                    L("DisplayName:Abp.Account.IsSelfRegistrationEnabled"), 
                    L("Description:Abp.Account.IsSelfRegistrationEnabled"), isVisibleToClients : true)
            );

            context.Add(
                new SettingDefinition(
                    AccountSettingNames.EnableLocalLogin, 
                    "true", 
                    L("DisplayName:Abp.Account.EnableLocalLogin"), 
                    L("Description:Abp.Account.EnableLocalLogin"), isVisibleToClients : true)
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AccountResource>(name);
        }
    }
}