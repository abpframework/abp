using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Http.Client
{
    public class AbpHttpClientBuilderOptions
    {
        public List<Action<string, IHttpClientBuilder>> ProxyClientBuildActions { get; }

        internal HashSet<string> ConfiguredProxyClients { get; }

        public AbpHttpClientBuilderOptions()
        {
            ProxyClientBuildActions = new List<Action<string, IHttpClientBuilder>>();
            ConfiguredProxyClients = new HashSet<string>();
        }
    }
}
