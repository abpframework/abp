using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity
{
    public class AbpIdentitySettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(IdentitySettingNames.Password.RequiredLength, "6"),
                new SettingDefinition(IdentitySettingNames.Password.RequiredUniqueChars, "1"),
                new SettingDefinition(IdentitySettingNames.Password.RequireNonAlphanumeric, "true"),
                new SettingDefinition(IdentitySettingNames.Password.RequireLowercase, "true"),
                new SettingDefinition(IdentitySettingNames.Password.RequireUppercase, "true"),
                new SettingDefinition(IdentitySettingNames.Password.RequireDigit, "true"),

                new SettingDefinition(IdentitySettingNames.Lockout.AllowedForNewUsers, "true"),
                new SettingDefinition(IdentitySettingNames.Lockout.LockoutDuration, "300"),
                new SettingDefinition(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, "5"),

                new SettingDefinition(IdentitySettingNames.SignIn.RequireConfirmedEmail, "false"),
                new SettingDefinition(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, "false"),

                new SettingDefinition(IdentitySettingNames.User.IsUserNameUpdateEnabled, "true"),
                new SettingDefinition(IdentitySettingNames.User.IsEmailUpdateEnabled, "true")
            );
        }
    }
}
