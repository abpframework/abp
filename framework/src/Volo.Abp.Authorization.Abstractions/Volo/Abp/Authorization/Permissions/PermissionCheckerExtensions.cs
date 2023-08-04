using System;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization.Permissions;

public static class PermissionCheckerExtensions
{
    public async static Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, Guid userId, [NotNull] string name)
    {
        return await permissionChecker.IsGrantedAsync(new ClaimsPrincipal(new ClaimsIdentity(new []{ new Claim(AbpClaimTypes.UserId, userId.ToString()) })), name);
    }

    public async static Task<MultiplePermissionGrantResult> IsGrantedAsync(this IPermissionChecker permissionChecker, Guid userId, [NotNull] string[] names)
    {
        return await permissionChecker.IsGrantedAsync(new ClaimsPrincipal(new ClaimsIdentity(new []{ new Claim(AbpClaimTypes.UserId, userId.ToString()) })), names);
    }
}
