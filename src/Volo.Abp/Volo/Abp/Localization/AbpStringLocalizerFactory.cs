using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Localization.Json;

namespace Volo.Abp.Localization
{
    public class AbpStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ResourceManagerStringLocalizerFactory _innerFactory;
        private readonly AbpLocalizationOptions _abpLocalizationOptions;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<Type, AbpDictionaryBasedStringLocalizer> _localizerCache;

        //TODO: It's better to use decorator pattern for IStringLocalizerFactory instead of getting ResourceManagerStringLocalizerFactory as a dependency.
        public AbpStringLocalizerFactory(
            ResourceManagerStringLocalizerFactory innerFactory,
            IOptions<AbpLocalizationOptions> abpLocalizationOptions, IServiceProvider serviceProvider)
        {
            _innerFactory = innerFactory;
            _serviceProvider = serviceProvider;
            _abpLocalizationOptions = abpLocalizationOptions.Value;

            _localizerCache = new ConcurrentDictionary<Type, AbpDictionaryBasedStringLocalizer>();;
        }

        public virtual IStringLocalizer Create(Type resourceSource)
        {
            //TODO: Optimize!

            var localizationResource = _abpLocalizationOptions.Resources.FirstOrDefault(l => l.ResourceType == resourceSource);
            if (localizationResource == null)
            {
                return _innerFactory.Create(resourceSource);
            }

            return _localizerCache.GetOrAdd(resourceSource, _ => CreateAbpStringLocalizer(localizationResource));
        }

        private AbpDictionaryBasedStringLocalizer CreateAbpStringLocalizer(LocalizationResource resource)
        {
            resource.Initialize(_serviceProvider);

            //Use JSON/XML/...etc based provider that reads resource from source and creates a dictionary
            //Extend dictionary with extensions
            //Wrap reader by wrappers (like db wrapper which implement multitenancy/regions and so on...)


            //Notes: Localizer will be cached, so wrappers are responsible to cache/invalidate themselves!

            var localizer = new AbpDictionaryBasedStringLocalizer(resource); //TODO: !!!

            //TODO: Wrap with DB provider or other premium sources

            return localizer;
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
    }
}