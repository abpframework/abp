using Volo.Abp.Ldap.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Volo.Abp.Ldap
{
    public class LdapSettingProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    LdapSettingNames.ServerHost,
                    "",
                    L("DisplayName:Abp.Ldap.ServerHost"),
                    L("Description:Abp.Ldap.ServerHost")),

                new SettingDefinition(
                    LdapSettingNames.ServerPort,
                    "",
                    L("DisplayName:Abp.Ldap.ServerPort"),
                    L("Description:Abp.Ldap.ServerPort")),

                new SettingDefinition(
                    LdapSettingNames.UseSsl,
                    "",
                    L("DisplayName:Abp.Ldap.UseSsl"),
                    L("Description:Abp.Ldap.UseSsl")),

                new SettingDefinition(
                    LdapSettingNames.SearchBase,
                    "",
                    L("DisplayName:Abp.Ldap.SearchBase"),
                    L("Description:Abp.Ldap.SearchBase")),

                new SettingDefinition(
                    LdapSettingNames.DomainName,
                    "",
                    L("DisplayName:Abp.Ldap.DomainName"),
                    L("Description:Abp.Ldap.DomainName")),

                new SettingDefinition(
                    LdapSettingNames.DomainDistinguishedName,
                    "",
                    L("DisplayName:Abp.Ldap.DomainDistinguishedName"),
                    L("Description:Abp.Ldap.DomainDistinguishedName")),

                new SettingDefinition(
                    LdapSettingNames.Credentials.DomainUserName,
                    "",
                    L("DisplayName:Abp.Ldap.Credentials.DomainUserName"),
                    L("Description:Abp.Ldap.Credentials.DomainUserName")),

                new SettingDefinition(
                    LdapSettingNames.Credentials.Password,
                    "",
                    L("DisplayName:Abp.Ldap.Credentials.Password"),
                    L("Description:Abp.Ldap.Credentials.Password"))
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LdapResource>(name);
        }
    }
}
