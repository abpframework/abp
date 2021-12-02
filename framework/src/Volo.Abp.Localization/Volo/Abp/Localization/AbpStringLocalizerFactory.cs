using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Localization;

public class AbpStringLocalizerFactory : IStringLocalizerFactory, IAbpStringLocalizerFactoryWithDefaultResourceSupport
{
    protected internal AbpLocalizationOptions AbpLocalizationOptions { get; }
    protected ResourceManagerStringLocalizerFactory InnerFactory { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected ConcurrentDictionary<Type, StringLocalizerCacheItem> LocalizerCache { get; }

    //TODO: It's better to use decorator pattern for IStringLocalizerFactory instead of getting ResourceManagerStringLocalizerFactory as a dependency.
    public AbpStringLocalizerFactory(
        ResourceManagerStringLocalizerFactory innerFactory,
        IOptions<AbpLocalizationOptions> abpLocalizationOptions,
        IServiceProvider serviceProvider)
    {
        InnerFactory = innerFactory;
        ServiceProvider = serviceProvider;
        AbpLocalizationOptions = abpLocalizationOptions.Value;

        LocalizerCache = new ConcurrentDictionary<Type, StringLocalizerCacheItem>();
    }

    public virtual IStringLocalizer Create(Type resourceType)
    {
        var resource = AbpLocalizationOptions.Resources.GetOrDefault(resourceType);
        if (resource == null)
        {
            return InnerFactory.Create(resourceType);
        }

        if (LocalizerCache.TryGetValue(resourceType, out var cacheItem))
        {
            return cacheItem.Localizer;
        }

        lock (LocalizerCache)
        {
            return LocalizerCache.GetOrAdd(
                resourceType,
                _ => CreateStringLocalizerCacheItem(resource)
            ).Localizer;
        }
    }

    private StringLocalizerCacheItem CreateStringLocalizerCacheItem(LocalizationResource resource)
    {
        foreach (var globalContributor in AbpLocalizationOptions.GlobalContributors)
        {
            resource.Contributors.Add((ILocalizationResourceContributor)Activator.CreateInstance(globalContributor));
        }

        var context = new LocalizationResourceInitializationContext(resource, ServiceProvider);

        foreach (var contributor in resource.Contributors)
        {
            contributor.Initialize(context);
        }

        return new StringLocalizerCacheItem(
            new AbpDictionaryBasedStringLocalizer(
                resource,
                resource.BaseResourceTypes.Select(Create).ToList()
            )
        );
    }

    public virtual IStringLocalizer Create(string baseName, string location)
    {
        //TODO: Investigate when this is called?

        return InnerFactory.Create(baseName, location);
    }

    internal static void Replace(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Singleton<IStringLocalizerFactory, AbpStringLocalizerFactory>());
        services.AddSingleton<ResourceManagerStringLocalizerFactory>();
    }

    protected class StringLocalizerCacheItem
    {
        public AbpDictionaryBasedStringLocalizer Localizer { get; }

        public StringLocalizerCacheItem(AbpDictionaryBasedStringLocalizer localizer)
        {
            Localizer = localizer;
        }
    }

    public IStringLocalizer CreateDefaultOrNull()
    {
        if (AbpLocalizationOptions.DefaultResourceType == null)
        {
            return null;
        }

        return Create(AbpLocalizationOptions.DefaultResourceType);
    }
}
