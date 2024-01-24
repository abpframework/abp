using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features;

public class FeatureValueProviderManager : IFeatureValueProviderManager, ISingletonDependency
{
    public IReadOnlyList<IFeatureValueProvider> ValueProviders => GetProviders();
    private readonly Lazy<List<IFeatureValueProvider>> _lazyProviders;

    protected AbpFeatureOptions Options { get; }

    public FeatureValueProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpFeatureOptions> options)
    {
        Options = options.Value;

        _lazyProviders = new Lazy<List<IFeatureValueProvider>>(
            () => Options
                .ValueProviders
                .Select(c => serviceProvider.GetRequiredService(c) as IFeatureValueProvider)
                .ToList()!,
            true
        );
    }
    
    protected virtual List<IFeatureValueProvider> GetProviders()
    {
        var providers = _lazyProviders.Value;
        
        var multipleProviders = providers.GroupBy(p => p.Name).FirstOrDefault(x => x.Count() > 1);
        if(multipleProviders != null)
        {
            throw new AbpException($"Duplicate feature value provider name detected: {multipleProviders.Key}. Providers:{Environment.NewLine}{multipleProviders.Select(p => p.GetType().FullName!).JoinAsString(Environment.NewLine)}");
        }

        return providers;
    }
}