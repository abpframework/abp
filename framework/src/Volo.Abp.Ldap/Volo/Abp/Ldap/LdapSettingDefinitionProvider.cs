using Volo.Abp.Ldap.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Volo.Abp.Ldap;

public class LdapSettingDefinitionProvider : SettingDefinitionProvider
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
                "389",
                L("DisplayName:Abp.Ldap.ServerPort"),
                L("Description:Abp.Ldap.ServerPort")),

            new SettingDefinition(
                LdapSettingNames.BaseDc,
                "",
                L("DisplayName:Abp.Ldap.BaseDc"),
                L("Description:Abp.Ldap.BaseDc")),

            new SettingDefinition(
                LdapSettingNames.Domain,
                "",
                L("DisplayName:Abp.Ldap.Domain"),
                L("Description:Abp.Ldap.Domain")),

            new SettingDefinition(
                LdapSettingNames.UserName,
                "",
                L("DisplayName:Abp.Ldap.UserName"),
                L("Description:Abp.Ldap.UserName")),

            new SettingDefinition(
                LdapSettingNames.Password,
                "",
                L("DisplayName:Abp.Ldap.Password"),
                L("Description:Abp.Ldap.Password"),
                isEncrypted: true)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LdapResource>(name);
    }
}
