using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity;

public class AbpIdentitySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                IdentitySettingNames.Password.RequiredLength,
                6.ToString(),
                L("DisplayName:Abp.Identity.Password.RequiredLength"),
                L("Description:Abp.Identity.Password.RequiredLength"),
                true),

            new SettingDefinition(
                IdentitySettingNames.Password.RequiredUniqueChars,
                1.ToString(),
                L("DisplayName:Abp.Identity.Password.RequiredUniqueChars"),
                L("Description:Abp.Identity.Password.RequiredUniqueChars"),
                true),

            new SettingDefinition(
                IdentitySettingNames.Password.RequireNonAlphanumeric,
                true.ToString(),
                L("DisplayName:Abp.Identity.Password.RequireNonAlphanumeric"),
                L("Description:Abp.Identity.Password.RequireNonAlphanumeric"),
                true),

            new SettingDefinition(
                IdentitySettingNames.Password.RequireLowercase,
                true.ToString(),
                L("DisplayName:Abp.Identity.Password.RequireLowercase"),
                L("Description:Abp.Identity.Password.RequireLowercase"),
                true),

            new SettingDefinition(
                IdentitySettingNames.Password.RequireUppercase,
                true.ToString(),
                L("DisplayName:Abp.Identity.Password.RequireUppercase"),
                L("Description:Abp.Identity.Password.RequireUppercase"),
                true),

            new SettingDefinition(
                IdentitySettingNames.Password.RequireDigit,
                true.ToString(),
                L("DisplayName:Abp.Identity.Password.RequireDigit"),
                L("Description:Abp.Identity.Password.RequireDigit"),
                true),

            new SettingDefinition(
                IdentitySettingNames.Lockout.AllowedForNewUsers,
                true.ToString(),
                L("DisplayName:Abp.Identity.Lockout.AllowedForNewUsers"),
                L("Description:Abp.Identity.Lockout.AllowedForNewUsers"),
                true),

            new SettingDefinition(
                IdentitySettingNames.Lockout.LockoutDuration,
                (5 * 60).ToString(),
                L("DisplayName:Abp.Identity.Lockout.LockoutDuration"),
                L("Description:Abp.Identity.Lockout.LockoutDuration"),
                true),

            new SettingDefinition(
                IdentitySettingNames.Lockout.MaxFailedAccessAttempts,
                5.ToString(),
                L("DisplayName:Abp.Identity.Lockout.MaxFailedAccessAttempts"),
                L("Description:Abp.Identity.Lockout.MaxFailedAccessAttempts"),
                true),

            new SettingDefinition(
                IdentitySettingNames.SignIn.RequireConfirmedEmail,
                false.ToString(),
                L("DisplayName:Abp.Identity.SignIn.RequireConfirmedEmail"),
                L("Description:Abp.Identity.SignIn.RequireConfirmedEmail"),
                true),
            new SettingDefinition(
                IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation,
                true.ToString(),
                L("DisplayName:Abp.Identity.SignIn.EnablePhoneNumberConfirmation"),
                L("Description:Abp.Identity.SignIn.EnablePhoneNumberConfirmation"),
                true),
            new SettingDefinition(
                IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber,
                false.ToString(),
                L("DisplayName:Abp.Identity.SignIn.RequireConfirmedPhoneNumber"),
                L("Description:Abp.Identity.SignIn.RequireConfirmedPhoneNumber"),
                true),

            new SettingDefinition(
                IdentitySettingNames.User.IsUserNameUpdateEnabled,
                true.ToString(),
                L("DisplayName:Abp.Identity.User.IsUserNameUpdateEnabled"),
                L("Description:Abp.Identity.User.IsUserNameUpdateEnabled"),
                true),

            new SettingDefinition(
                IdentitySettingNames.User.IsEmailUpdateEnabled,
                true.ToString(),
                L("DisplayName:Abp.Identity.User.IsEmailUpdateEnabled"),
                L("Description:Abp.Identity.User.IsEmailUpdateEnabled"),
                true),

            new SettingDefinition(
                IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount,
                int.MaxValue.ToString(),
                L("Identity.OrganizationUnit.MaxUserMembershipCount"),
                L("Identity.OrganizationUnit.MaxUserMembershipCount"),
                true)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
