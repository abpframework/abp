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
                new SettingDefinition(EmailSettingNames.Smtp.Host, "127.0.0.1"),
                new SettingDefinition(EmailSettingNames.Smtp.Port, "25"),
                new SettingDefinition(EmailSettingNames.Smtp.UserName),
                new SettingDefinition(EmailSettingNames.Smtp.Password),
                new SettingDefinition(EmailSettingNames.Smtp.Domain),
                new SettingDefinition(EmailSettingNames.Smtp.EnableSsl, "false"),
                new SettingDefinition(EmailSettingNames.Smtp.UseDefaultCredentials, "true"),
                new SettingDefinition(EmailSettingNames.DefaultFromAddress),
                new SettingDefinition(EmailSettingNames.DefaultFromDisplayName)
            );
        }
    }
}