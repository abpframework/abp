using System;
using System.Collections.Generic;
using Volo.Abp.Http.DynamicProxying;

namespace Volo.Abp.Http
{
    public class AbpHttpOptions
    {
        public Dictionary<Type, DynamicHttpClientProxyConfig> HttpClientProxies { get; set; }

        public AbpHttpOptions()
        {
            HttpClientProxies = new Dictionary<Type, DynamicHttpClientProxyConfig>();
        }
    }
}