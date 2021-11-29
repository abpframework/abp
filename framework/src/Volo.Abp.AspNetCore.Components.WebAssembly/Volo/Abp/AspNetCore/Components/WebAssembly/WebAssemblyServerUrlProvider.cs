using System;
using System.Threading.Tasks;
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
        
        public async Task<string> GetBaseUrlAsync(string remoteServiceName = null)
        {
            var remoteServiceConfiguration = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(
                remoteServiceName ?? RemoteServiceConfigurationDictionary.DefaultName
            );
            
            return remoteServiceConfiguration.BaseUrl.EnsureEndsWith('/');
        }
    }
}