using System;
using System.Linq;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.IdentityModel;

[Serializable]
public class IdentityModelTokenCacheItem
{
    public string AccessToken { get; set; }

    public IdentityModelTokenCacheItem()
    {

    }

    public IdentityModelTokenCacheItem(string accessToken)
    {
        AccessToken = accessToken;
    }

    public static string CalculateCacheKey(IdentityClientConfiguration configuration)
    {
        return string.Join(",", configuration.Select(x => x.Key + ":" + x.Value)).ToMd5();
    }
}
