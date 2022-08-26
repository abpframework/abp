using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Localization;

public class AbpStringLocalizerFactory : IStringLocalizerFactory, IAbpStringLocalizerFactory
{
    protected internal AbpLocalizationOptions AbpLocalizationOptions { get; }
    protected ResourceManagerStringLocalizerFactory InnerFactory { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected IExternalLocalizationStore ExternalLocalizationStore { get; }
    protected ConcurrentDictionary<string, StringLocalizerCacheItem> LocalizerCache { get; }

    //TODO: It's better to use decorator pattern for IStringLocalizerFactory instead of getting ResourceManagerStringLocalizerFactory as a dependency.
    public AbpStringLocalizerFactory(
        ResourceManagerStringLocalizerFactory innerFactory,
        IOptions<AbpLocalizationOptions> abpLocalizationOptions,
        IServiceProvider serviceProvider,
        IExternalLocalizationStore externalLocalizationStore)
    {
        InnerFactory = innerFactory;
        ServiceProvider = serviceProvider;
        ExternalLocalizationStore = externalLocalizationStore;
        AbpLocalizationOptions = abpLocalizationOptions.Value;

        LocalizerCache = new ConcurrentDictionary<string, StringLocalizerCacheItem>();
    }

    public virtual IStringLocalizer Create(Type resourceType)
    {
        var resource = AbpLocalizationOptions.Resources.GetOrNull(resourceType);
        if (resource == null)
        {
            return InnerFactory.Create(resourceType);
        }

        return CreateInternal(resource.ResourceName, resource);
    }
    
    public IStringLocalizer CreateByResourceNameOrNull(string resourceName)
    {
        var resource = AbpLocalizationOptions.Resources.GetOrDefault(resourceName);
        if (resource == null)
        {
            resource = ExternalLocalizationStore.GetResourceOrNull(resourceName);
            if (resource == null)
            {
                return null;
            }
        }

        return CreateInternal(resourceName, resource);
    }
    
    private IStringLocalizer CreateInternal(string resourceName, LocalizationResourceBase resource)
    {
        if (LocalizerCache.TryGetValue(resourceName, out var cacheItem))
        {
            return cacheItem.Localizer;
        }

        lock (LocalizerCache)
        {
            return LocalizerCache.GetOrAdd(
                resourceName,
                _ => CreateStringLocalizerCacheItem(resource)
            ).Localizer;
        }
    }

    private StringLocalizerCacheItem CreateStringLocalizerCacheItem(LocalizationResourceBase resource)
    {
        foreach (var globalContributorType in AbpLocalizationOptions.GlobalContributors)
        {
            resource.Contributors.Add(
                Activator
                    .CreateInstance(globalContributorType)
                    .As<ILocalizationResourceContributor>()
            );
        }

        var context = new LocalizationResourceInitializationContext(resource, ServiceProvider);

        foreach (var contributor in resource.Contributors)
        {
            contributor.Initialize(context);
        }

        return new StringLocalizerCacheItem(
            new AbpDictionaryBasedStringLocalizer(
                resource,
                resource
                    .BaseResourceNames
                    .Select(CreateByResourceNameOrNull)
                    .Where(x => x != null)
                    .ToList(),
                AbpLocalizationOptions
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