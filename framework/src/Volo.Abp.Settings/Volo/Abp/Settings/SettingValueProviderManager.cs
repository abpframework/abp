using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings;

public class SettingValueProviderManager : ISettingValueProviderManager, ISingletonDependency
{
    public List<ISettingValueProvider> Providers => _lazyProviders.Value;

    protected AbpSettingOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }
    private readonly Lazy<List<ISettingValueProvider>> _lazyProviders;

    public SettingValueProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpSettingOptions> options)
    {

        Options = options.Value;
        ServiceProvider = serviceProvider;

        _lazyProviders = new Lazy<List<ISettingValueProvider>>(GetProviders, true);
    }
    
    protected virtual List<ISettingValueProvider> GetProviders()
    {
        var providers = Options
            .ValueProviders
            .Select(type => (ServiceProvider.GetRequiredService(type) as ISettingValueProvider)!)
            .ToList();
        
        var multipleProviders = providers.GroupBy(p => p.Name).FirstOrDefault(x => x.Count() > 1);
        if(multipleProviders != null)
        {
            throw new AbpException($"Duplicate setting value provider name detected: {multipleProviders.Key}. Providers:{Environment.NewLine}{multipleProviders.Select(p => p.GetType().FullName!).JoinAsString(Environment.NewLine)}");
        }

        return providers;
    }
}
