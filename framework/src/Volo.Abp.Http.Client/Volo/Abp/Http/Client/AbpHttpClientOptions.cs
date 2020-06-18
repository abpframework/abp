using System;
using System.Collections.Generic;
using System.Net.Http;
using Volo.Abp.Http.Client.DynamicProxying;

namespace Volo.Abp.Http.Client
{
    public class AbpHttpClientOptions
    {
        public Dictionary<Type, DynamicHttpClientProxyConfig> HttpClientProxies { get; set; }

        public List<Func<string, Action<HttpClient>>> HttpClientActions { get; }

        public AbpHttpClientOptions()
        {
            HttpClientProxies = new Dictionary<Type, DynamicHttpClientProxyConfig>();
            HttpClientActions = new List<Func<string, Action<HttpClient>>>();
        }
    }
}
