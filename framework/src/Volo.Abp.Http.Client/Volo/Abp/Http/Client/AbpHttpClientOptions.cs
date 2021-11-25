using System;
using System.Collections.Generic;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Http.Client.Proxying;

namespace Volo.Abp.Http.Client;

public class AbpHttpClientOptions
{
    public Dictionary<Type, HttpClientProxyConfig> HttpClientProxies { get; set; }

    public AbpHttpClientOptions()
    {
        HttpClientProxies = new Dictionary<Type, HttpClientProxyConfig>();
    }
}
