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

                new SettingDefinition(IdentitySettingNames.User.IsUserNameUpdateEnabled),
                new SettingDefinition(IdentitySettingNames.User.IsEmailUpdateEnabled)

            );
        }
    }
}
