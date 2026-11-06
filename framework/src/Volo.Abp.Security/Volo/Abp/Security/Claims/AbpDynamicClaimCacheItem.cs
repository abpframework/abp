using System;
using System.Collections.Generic;

namespace Volo.Abp.Security.Claims;

[Serializable]
public class AbpDynamicClaimCacheItem
{
    public List<AbpDynamicClaim> Claims { get; set; }

    public AbpDynamicClaimCacheItem()
    {
        Claims = new List<AbpDynamicClaim>();
    }

    public AbpDynamicClaimCacheItem(List<AbpDynamicClaim> claims)
    {
        Claims = claims;
    }

    public static string CalculateCacheKey(Guid userId, Guid? tenantId)
    {
        return $"{tenantId}-{userId}";
    }

    public static string CalculateRevokedSessionCacheKey(string sessionId)
    {
        return $"revoked-session:{sessionId}";
    }
}
