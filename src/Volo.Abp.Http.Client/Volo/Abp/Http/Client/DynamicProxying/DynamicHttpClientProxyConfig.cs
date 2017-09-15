using System;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class DynamicHttpClientProxyConfig
    {
        public string BaseUrl { get; }

        public string ModuleName { get; }

        public Type Type { get; }

        public DynamicHttpClientProxyConfig(string moduleName, string baseUrl, Type type)
        {
            BaseUrl = baseUrl;
            ModuleName = moduleName;
            Type = type;
        }
    }
}