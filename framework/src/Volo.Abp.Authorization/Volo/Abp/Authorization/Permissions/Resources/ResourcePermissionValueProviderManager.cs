using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions.Resources;

public class ResourcePermissionValueProviderManager : IResourcePermissionValueProviderManager, ISingletonDependency
{
    public IReadOnlyList<IResourcePermissionValueProvider> ValueProviders => _lazyProviders.Value;
    private readonly Lazy<List<IResourcePermissionValueProvider>> _lazyProviders;

    protected AbpPermissionOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }

    public ResourcePermissionValueProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpPermissionOptions> options)
    {
        Options = options.Value;
        ServiceProvider = serviceProvider;

        _lazyProviders = new Lazy<List<IResourcePermissionValueProvider>>(GetProviders, true);
    }

    protected virtual List<IResourcePermissionValueProvider> GetProviders()
    {
        var providers = Options
            .ResourceValueProviders
            .Select(type => (ServiceProvider.GetRequiredService(type) as IResourcePermissionValueProvider)!)
            .ToList();

        var multipleProviders = providers.GroupBy(p => p.Name).FirstOrDefault(x => x.Count() > 1);
        if(multipleProviders != null)
        {
            throw new AbpException($"Duplicate resource permission value provider name detected: {multipleProviders.Key}. Providers:{Environment.NewLine}{multipleProviders.Select(p => p.GetType().FullName!).JoinAsString(Environment.NewLine)}");
        }

        return providers;
    }
}
