using System;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class DynamicHttpClientProxyConfig
    {
        public string BaseUrl { get; }

        public Type Type { get; }

        public DynamicHttpClientProxyConfig(string baseUrl, Type type)
        {
            BaseUrl = baseUrl;
            Type = type;
        }
    }
}