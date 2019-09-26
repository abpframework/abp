using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity
{
    public class AbpIdentitySettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(IdentitySettingNames.Password.RequiredLength, null, null, null, true),
                new SettingDefinition(IdentitySettingNames.Password.RequiredUniqueChars, null, null, null, true),
                new SettingDefinition(IdentitySettingNames.Password.RequireNonAlphanumeric, null, null, null, true),
                new SettingDefinition(IdentitySettingNames.Password.RequireLowercase, null, null, null, true),
                new SettingDefinition(IdentitySettingNames.Password.RequireUppercase, null, null, null, true),
                new SettingDefinition(IdentitySettingNames.Password.RequireDigit, null, null, null, true),

                new SettingDefinition(IdentitySettingNames.Lockout.AllowedForNewUsers, null, null, null, true),
                new SettingDefinition(IdentitySettingNames.Lockout.LockoutDuration, null, null, null, true),
                new SettingDefinition(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, null, null, null, true),

                new SettingDefinition(IdentitySettingNames.SignIn.RequireConfirmedEmail, null, null, null, true),
                new SettingDefinition(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, null, null, null, true),

                new SettingDefinition(IdentitySettingNames.User.IsUserNameUpdateEnabled, "true", null, null, true),
                new SettingDefinition(IdentitySettingNames.User.IsEmailUpdateEnabled, "true", null, null, true)

            );
        }
    }
}
