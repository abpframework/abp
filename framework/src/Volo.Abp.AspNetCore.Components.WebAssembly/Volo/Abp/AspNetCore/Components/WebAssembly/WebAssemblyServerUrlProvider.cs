using System;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    [Dependency(ReplaceServices = true)]
    public class WebAssemblyServerUrlProvider : IServerUrlProvider, ITransientDependency
    {
        protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; }

        public WebAssemblyServerUrlProvider(
            IRemoteServiceConfigurationProvider remoteServiceConfigurationProvider)
        {
            RemoteServiceConfigurationProvider = remoteServiceConfigurationProvider;
        }
        
        public string GetBaseUrl(string remoteServiceName = null)
        {
            return RemoteServiceConfigurationProvider.GetConfigurationOrDefault(
                remoteServiceName ?? RemoteServiceConfigurationDictionary.DefaultName
            ).BaseUrl.EnsureEndsWith('/');
        }
    }
}