using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions;

public class PermissionValueProviderManager : IPermissionValueProviderManager, ISingletonDependency
{
    public IReadOnlyList<IPermissionValueProvider> ValueProviders => GetProviders();
    private readonly Lazy<List<IPermissionValueProvider>> _lazyProviders;

    protected AbpPermissionOptions Options { get; }

    public PermissionValueProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpPermissionOptions> options)
    {
        Options = options.Value;

        _lazyProviders = new Lazy<List<IPermissionValueProvider>>(
            () => Options
                .ValueProviders
                .Select(c => serviceProvider.GetRequiredService(c) as IPermissionValueProvider)
                .ToList()!,
            true
        );
    }
    
    protected virtual List<IPermissionValueProvider> GetProviders()
    {
        var providers = _lazyProviders.Value;
        
        var multipleProviders = providers.GroupBy(p => p.Name).FirstOrDefault(x => x.Count() > 1);
        if(multipleProviders != null)
        {
            throw new AbpException($"Duplicate permission value provider name detected: {multipleProviders.Key}. Providers:{Environment.NewLine}{multipleProviders.Select(p => p.GetType().FullName!).JoinAsString(Environment.NewLine)}");
        }

        return providers;
    }
}
