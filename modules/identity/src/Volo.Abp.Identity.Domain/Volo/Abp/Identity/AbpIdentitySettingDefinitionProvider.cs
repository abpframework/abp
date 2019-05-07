using Volo.Abp.Emailing;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity
{
    public class AbpIdentitySettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(

                new SettingDefinition(IdentitySettingNames.Password.RequiredLength),
                new SettingDefinition(IdentitySettingNames.Password.RequiredUniqueChars),
                new SettingDefinition(IdentitySettingNames.Password.RequireNonAlphanumeric),
                new SettingDefinition(IdentitySettingNames.Password.RequireLowercase),
                new SettingDefinition(IdentitySettingNames.Password.RequireUppercase),
                new SettingDefinition(IdentitySettingNames.Password.RequireDigit),

                new SettingDefinition(IdentitySettingNames.Lockout.AllowedForNewUsers),
                new SettingDefinition(IdentitySettingNames.Lockout.LockoutDuration),
                new SettingDefinition(IdentitySettingNames.Lockout.MaxFailedAccessAttempts),

                new SettingDefinition(IdentitySettingNames.SignIn.RequireConfirmedEmail),
                new SettingDefinition(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber),

                new SettingDefinition(IdentitySettingNames.User.IsUserNameUpdateEnabled, "true"),
                new SettingDefinition(IdentitySettingNames.User.IsEmailUpdateEnabled, "true"),

                new SettingDefinition(EmailSettingNames.Smtp.Host, "smtp.163.com"),
                new SettingDefinition(EmailSettingNames.Smtp.Port, "25"),
                new SettingDefinition(EmailSettingNames.Smtp.UserName, "igeekfan@163.com"),
                new SettingDefinition(EmailSettingNames.Smtp.Password, "abc520", isEncrypted: true),
                new SettingDefinition(EmailSettingNames.Smtp.Domain),
                new SettingDefinition(EmailSettingNames.Smtp.EnableSsl, "true"),
                new SettingDefinition(EmailSettingNames.Smtp.UseDefaultCredentials, "true"),
                new SettingDefinition(EmailSettingNames.DefaultFromAddress, "igeekfan@163.com"),
                new SettingDefinition(EmailSettingNames.DefaultFromDisplayName, "ABP application")

            );
        }
    }
}
