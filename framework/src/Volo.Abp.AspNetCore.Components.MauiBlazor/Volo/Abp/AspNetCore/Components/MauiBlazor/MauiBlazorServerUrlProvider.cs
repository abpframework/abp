using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor;

[Dependency(ReplaceServices = true)]
public class MauiBlazorServerUrlProvider : IServerUrlProvider, ITransientDependency
{
    protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; }

    public MauiBlazorServerUrlProvider(
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
