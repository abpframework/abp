using System;
using System.Linq;
using IdentityModel.Client;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.IdentityModel
{
    [Serializable]
    [IgnoreMultiTenancy]
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

        public static string CalculateCacheKey(DiscoveryDocumentResponse discoveryResponse, IdentityClientConfiguration configuration)
        {
            return discoveryResponse.TokenEndpoint + string.Join(",", configuration.Select(x => x.Key + ":" + x.Value));
        }
    }
}
