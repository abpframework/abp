using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings;

public class SettingValueProviderManager : ISettingValueProviderManager, ISingletonDependency
{
    public List<ISettingValueProvider> Providers => GetProviders();

    protected AbpSettingOptions Options { get; }
    private readonly Lazy<List<ISettingValueProvider>> _lazyProviders;

    public SettingValueProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpSettingOptions> options)
    {

        Options = options.Value;

        _lazyProviders = new Lazy<List<ISettingValueProvider>>(
            () => Options
                .ValueProviders
                .Select(type => serviceProvider.GetRequiredService(type) as ISettingValueProvider)
                .ToList()!,
            true
        );
    }
    
    protected virtual List<ISettingValueProvider> GetProviders()
    {
        var providers = _lazyProviders.Value;
        
        var multipleProviders = providers.GroupBy(p => p.Name).FirstOrDefault(x => x.Count() > 1);
        if(multipleProviders != null)
        {
            throw new AbpException($"Duplicate setting value provider name detected: {multipleProviders.Key}. Providers:{Environment.NewLine}{multipleProviders.Select(p => p.GetType().FullName!).JoinAsString(Environment.NewLine)}");
        }

        return providers;
    }
}
