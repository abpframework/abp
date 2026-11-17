using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;

namespace Bench.Identity;

public class ChangeIdentityPasswordPolicySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        var requireNonAlphanumeric = context.GetOrNull(IdentitySettingNames.Password.RequireNonAlphanumeric);
        if (requireNonAlphanumeric != null)
        {
            requireNonAlphanumeric.DefaultValue = false.ToString();
        }

        var requireLowercase = context.GetOrNull(IdentitySettingNames.Password.RequireLowercase);
        if (requireLowercase != null)
        {
            requireLowercase.DefaultValue = false.ToString();
        }

        var requireUppercase = context.GetOrNull(IdentitySettingNames.Password.RequireUppercase);
        if (requireUppercase != null)
        {
            requireUppercase.DefaultValue = false.ToString();
        }

        var requireDigit = context.GetOrNull(IdentitySettingNames.Password.RequireDigit);
        if (requireDigit != null)
        {
            requireDigit.DefaultValue = false.ToString();
        }
    }
}
