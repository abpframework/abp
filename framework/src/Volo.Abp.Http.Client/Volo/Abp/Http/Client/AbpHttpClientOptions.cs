using System;
using System.Collections.Generic;
using Volo.Abp.Http.Client.DynamicProxying;

namespace Volo.Abp.Http.Client
{
    public class AbpHttpClientOptions
    {
        public Dictionary<Type, DynamicHttpClientProxyConfig> HttpClientProxies { get; set; }

        public AbpHttpClientOptions()
        {
            HttpClientProxies = new Dictionary<Type, DynamicHttpClientProxyConfig>();
        }
    }
}
