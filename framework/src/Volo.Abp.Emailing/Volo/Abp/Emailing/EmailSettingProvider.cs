using Volo.Abp.Emailing.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Volo.Abp.Emailing
{
    /// <summary>
    /// Defines settings to send emails.
    /// <see cref="EmailSettingNames"/> for all available configurations.
    /// </summary>
    internal class EmailSettingProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    EmailSettingNames.Smtp.Host, 
                    "127.0.0.1", 
                    L("DisplayName:Abp.Mailing.Smtp.Host"), 
                    L("Description:Abp.Mailing.Smtp.Host")),

                new SettingDefinition(EmailSettingNames.Smtp.Port, 
                    "25", 
                    L("DisplayName:Abp.Mailing.Smtp.Port"), 
                    L("Description:Abp.Mailing.Smtp.Port")),

                new SettingDefinition(
                    EmailSettingNames.Smtp.UserName, 
                    displayName: L("DisplayName:Abp.Mailing.Smtp.UserName"), 
                    description: L("Description:Abp.Mailing.Smtp.UserName")),

                new SettingDefinition(
                    EmailSettingNames.Smtp.Password, 
                    displayName: 
                    L("DisplayName:Abp.Mailing.Smtp.Password"), 
                    description: L("Description:Abp.Mailing.Smtp.Password"), 
                    isEncrypted: true),

                new SettingDefinition(
                    EmailSettingNames.Smtp.Domain, 
                    displayName: L("DisplayName:Abp.Mailing.Smtp.Domain"), 
                    description: L("Description:Abp.Mailing.Smtp.Domain")),

                new SettingDefinition(
                    EmailSettingNames.Smtp.EnableSsl, 
                    "false",
                    L("DisplayName:Abp.Mailing.Smtp.EnableSsl"), 
                    L("Description:Abp.Mailing.Smtp.EnableSsl")),

                new SettingDefinition(
                    EmailSettingNames.Smtp.UseDefaultCredentials, 
                    "true", 
                    L("DisplayName:Abp.Mailing.Smtp.UseDefaultCredentials"), 
                    L("Description:Abp.Mailing.Smtp.UseDefaultCredentials")),

                new SettingDefinition(
                    EmailSettingNames.DefaultFromAddress, 
                    "noreply@abp.io", 
                    L("DisplayName:Abp.Mailing.DefaultFromAddress"), 
                    L("Description:Abp.Mailing.DefaultFromAddress")),

                new SettingDefinition(EmailSettingNames.DefaultFromDisplayName, 
                    "ABP application", 
                    L("DisplayName:Abp.Mailing.DefaultFromDisplayName"),
                    L("Description:Abp.Mailing.DefaultFromDisplayName"))
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<EmailingResource>(name);
        }
    }
}