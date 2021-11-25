using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.IdentityModel;

[Serializable]
[IgnoreMultiTenancy]
public class IdentityModelDiscoveryDocumentCacheItem
{
    public string TokenEndpoint { get; set; }

    public IdentityModelDiscoveryDocumentCacheItem()
    {

    }

    public IdentityModelDiscoveryDocumentCacheItem(string tokenEndpoint)
    {
        TokenEndpoint = tokenEndpoint;
    }

    public static string CalculateCacheKey(IdentityClientConfiguration configuration)
    {
        return configuration.Authority.ToLower().ToMd5();
    }
}
