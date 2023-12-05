using Microsoft.AspNetCore.Identity;
using static Volo.Abp.Identity.AspNetCore.AbpSecurityStampValidatorCallback;

namespace Volo.Abp.Identity.AspNetCore;

public static class SecurityStampValidatorOptionsExtensions
{
    public static SecurityStampValidatorOptions UpdatePrincipal(this SecurityStampValidatorOptions options)
    {
        var previousOnRefreshingPrincipal = options.OnRefreshingPrincipal;
        options.OnRefreshingPrincipal = async context =>
        {
            await SecurityStampValidatorCallback.UpdatePrincipal(context);
            await previousOnRefreshingPrincipal?.Invoke(context);
        };
        return options;
    }
}
