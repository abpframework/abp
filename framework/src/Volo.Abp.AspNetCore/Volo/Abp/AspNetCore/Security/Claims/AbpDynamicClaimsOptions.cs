using System.Collections.Generic;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Security.Claims;

public class AbpDynamicClaimsOptions
{
    /// <summary>
    /// List of the claims that will be dynamically added/overriden to the current principal.
    /// Default values are:
    /// - <see cref="AbpClaimTypes.UserName"/>
    /// - <see cref="AbpClaimTypes.Role"/>
    /// - <see cref="AbpClaimTypes.Email"/>
    /// - <see cref="AbpClaimTypes.EmailVerified"/>
    /// - <see cref="AbpClaimTypes.PhoneNumber"/>
    /// - <see cref="AbpClaimTypes.PhoneNumberVerified"/>
    /// </summary>
    public List<string> DynamicClaims { get; } = new();
    
    public string RemoteRefreshUrl { get; set; } = "/api/account/refresh-dynamic-claims";

    public AbpDynamicClaimsOptions()
    {
        DynamicClaims.Add(AbpClaimTypes.UserName);
        DynamicClaims.Add(AbpClaimTypes.Role);
        DynamicClaims.Add(AbpClaimTypes.Email);
        DynamicClaims.Add(AbpClaimTypes.EmailVerified);
        DynamicClaims.Add(AbpClaimTypes.PhoneNumber);
        DynamicClaims.Add(AbpClaimTypes.PhoneNumberVerified);
    }
}