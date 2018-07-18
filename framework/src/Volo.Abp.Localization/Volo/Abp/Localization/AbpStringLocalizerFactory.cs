using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Localization
{
    public class AbpStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ResourceManagerStringLocalizerFactory _innerFactory;
        private readonly AbpLocalizationOptions _abpLocalizationOptions;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<Type, StringLocalizerCacheItem> _localizerCache;

        //TODO: It's better to use decorator pattern for IStringLocalizerFactory instead of getting ResourceManagerStringLocalizerFactory as a dependency.
        public AbpStringLocalizerFactory(
            ResourceManagerStringLocalizerFactory innerFactory,
            IOptions<AbpLocalizationOptions> abpLocalizationOptions,
            IServiceProvider serviceProvider)
        {
            _innerFactory = innerFactory;
            _serviceProvider = serviceProvider;
            _abpLocalizationOptions = abpLocalizationOptions.Value;

            _localizerCache = new ConcurrentDictionary<Type, StringLocalizerCacheItem>();
        }

        public virtual IStringLocalizer Create(Type resourceType)
        {
            var resource = _abpLocalizationOptions.Resources.GetOrDefault(resourceType);
            if (resource == null)
            {
                return _innerFactory.Create(resourceType);
            }

            return _localizerCache.GetOrAdd(
                resourceType,
                _ => CreateStringLocalizerCacheItem(resource)
            ).Localizer;
        }

        private StringLocalizerCacheItem CreateStringLocalizerCacheItem(LocalizationResource resource)
        {
            foreach (var globalContributor in _abpLocalizationOptions.GlobalContributors)
            {
                resource.Contributors.Add((ILocalizationResourceContributor) Activator.CreateInstance(globalContributor));
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = new LocalizationResourceInitializationContext(resource, scope.ServiceProvider);

                foreach (var contributor in resource.Contributors)
                {
                    contributor.Initialize(context);
                }
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

            return _innerFactory.Create(baseName, location);
        }

        internal static void Replace(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton<IStringLocalizerFactory, AbpStringLocalizerFactory>());
            services.AddSingleton<ResourceManagerStringLocalizerFactory>();
        }

        private class StringLocalizerCacheItem
        {
            public AbpDictionaryBasedStringLocalizer Localizer { get; }

            public StringLocalizerCacheItem(AbpDictionaryBasedStringLocalizer localizer)
            {
                Localizer = localizer;
            }
        }
    }
}