using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement;

public class ConfigurationFeatureManagementProvider : IFeatureManagementProvider, ISingletonDependency
{
    public string Name => ConfigurationFeatureValueProvider.ProviderName;

    protected IConfiguration Configuration { get; }

    public ConfigurationFeatureManagementProvider(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public virtual bool Compatible(string providerName)
    {
        return providerName == Name;
    }

    public virtual Task<IAsyncDisposable> HandleContextAsync(string providerName, string providerKey)
    {
        return Task.FromResult<IAsyncDisposable>(NullAsyncDisposable.Instance);
    }

    public virtual Task<string> GetOrNullAsync(FeatureDefinition feature, string providerKey)
    {
        return Task.FromResult(Configuration[ConfigurationFeatureValueProvider.ConfigurationNamePrefix + feature.Name]);
    }

    public virtual Task SetAsync(FeatureDefinition feature, string value, string providerKey)
    {
        throw new AbpException($"Can not set a feature value to the application configuration.");
    }

    public virtual Task ClearAsync(FeatureDefinition feature, string providerKey)
    {
        throw new AbpException($"Can not set a feature value to the application configuration.");
    }
}
