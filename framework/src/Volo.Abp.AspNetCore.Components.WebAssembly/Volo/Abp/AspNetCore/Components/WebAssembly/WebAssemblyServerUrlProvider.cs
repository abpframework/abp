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
        public AbpRemoteServiceOptions Options { get; }

        public WebAssemblyServerUrlProvider(
            IOptions<AbpRemoteServiceOptions> options)
        {
            Options = options.Value;
        }
        
        public string GetBaseUrl(string remoteServiceName = null)
        {
            return Options.RemoteServices.GetConfigurationOrDefault(
                remoteServiceName ?? RemoteServiceConfigurationDictionary.DefaultName
            ).BaseUrl.EnsureEndsWith('/');
        }
    }
}