using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity
{
    public class AbpIdentitySettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(IdentitySettingNames.Password.RequiredLength, 6.ToString(), null, null, true),
                new SettingDefinition(IdentitySettingNames.Password.RequiredUniqueChars, 1.ToString(), null, null, true),
                new SettingDefinition(IdentitySettingNames.Password.RequireNonAlphanumeric, true.ToString(), null, null, true),
                new SettingDefinition(IdentitySettingNames.Password.RequireLowercase, true.ToString(), null, null, true),
                new SettingDefinition(IdentitySettingNames.Password.RequireUppercase, true.ToString(), null, null, true),
                new SettingDefinition(IdentitySettingNames.Password.RequireDigit, true.ToString(), null, null, true),

                new SettingDefinition(IdentitySettingNames.Lockout.AllowedForNewUsers, true.ToString(), null, null, true),
                new SettingDefinition(IdentitySettingNames.Lockout.LockoutDuration, (5*60).ToString(), null, null, true),
                new SettingDefinition(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, 5.ToString(), null, null, true),

                new SettingDefinition(IdentitySettingNames.SignIn.RequireConfirmedEmail, false.ToString(), null, null, true),
                new SettingDefinition(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, false.ToString(), null, null, true),

                new SettingDefinition(IdentitySettingNames.User.IsUserNameUpdateEnabled, true.ToString(), null, null, true),
                new SettingDefinition(IdentitySettingNames.User.IsEmailUpdateEnabled, true.ToString(), null, null, true)
            );
        }
    }
}
