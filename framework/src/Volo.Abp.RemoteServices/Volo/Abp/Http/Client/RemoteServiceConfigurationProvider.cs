using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Http.Client;

public class RemoteServiceConfigurationProvider : IRemoteServiceConfigurationProvider, IScopedDependency
{
    protected AbpRemoteServiceOptions Options { get; }
    protected IMultiTenantUrlProvider MultiTenantUrlProvider { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public RemoteServiceConfigurationProvider(
        IOptionsMonitor<AbpRemoteServiceOptions> options,
        IMultiTenantUrlProvider multiTenantUrlProvider, 
        ICurrentTenant currentTenant)
    {
        MultiTenantUrlProvider = multiTenantUrlProvider;
        CurrentTenant = currentTenant;
        Options = options.CurrentValue;
    }

    public virtual async Task<RemoteServiceConfiguration> GetConfigurationOrDefaultAsync(string name)
    {
        return (await GetMultiTenantConfigurationAsync(Options.RemoteServices.GetConfigurationOrDefault(name)))!;
    }

    public virtual async Task<RemoteServiceConfiguration?> GetConfigurationOrDefaultOrNullAsync(string name)
    {
        return await GetMultiTenantConfigurationAsync(Options.RemoteServices.GetConfigurationOrDefaultOrNull(name));
    }

    protected virtual async Task<RemoteServiceConfiguration?> GetMultiTenantConfigurationAsync(RemoteServiceConfiguration? configuration)
    {
        if (configuration == null)
        {
            return configuration;
        }

        var baseUrl = await MultiTenantUrlProvider.GetUrlAsync(configuration.BaseUrl);
        if (baseUrl == configuration.BaseUrl)
        {
            return configuration;
        }
        
        var multiTenantConfiguration = new RemoteServiceConfiguration(configuration)
        {
            BaseUrl = baseUrl
        };
        
        return multiTenantConfiguration;
    }
}
