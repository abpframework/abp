using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Http.Client
{
    public class AbpHttpClientBuilderOptions
    {
        public List<Action<string, IHttpClientBuilder>> ProxyClientBuildActions { get; }

        internal HashSet<string> ConfiguredProxyClients { get; }

        public List<Action<string, IServiceProvider, HttpClient>> ProxyClientActions { get; }

        public AbpHttpClientBuilderOptions()
        {
            ProxyClientBuildActions = new List<Action<string, IHttpClientBuilder>>();
            ConfiguredProxyClients = new HashSet<string>();
            ProxyClientActions = new List<Action<string, IServiceProvider, HttpClient>>();
        }
    }
}
