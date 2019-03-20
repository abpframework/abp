using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureManager : IFeatureManager, ISingletonDependency
    {
        protected IFeatureDefinitionManager FeatureDefinitionManager { get; }
        protected List<IFeatureManagementProvider> Providers => _lazyProviders.Value;
        protected FeatureManagementOptions Options { get; }

        private readonly Lazy<List<IFeatureManagementProvider>> _lazyProviders;

        public FeatureManager(
            IOptions<FeatureManagementOptions> options,
            IServiceProvider serviceProvider,
            IFeatureDefinitionManager featureDefinitionManager)
        {
            FeatureDefinitionManager = featureDefinitionManager;
            Options = options.Value;

            //TODO: Instead, use IHybridServiceScopeFactory and create a scope..?

            _lazyProviders = new Lazy<List<IFeatureManagementProvider>>(
                () => Options
                    .Providers
                    .Select(c => serviceProvider.GetRequiredService(c) as IFeatureManagementProvider)
                    .ToList(),
                true
            );
        }
        
        public virtual Task<string> GetOrNullAsync(
            string name, 
            string providerName, 
            string providerKey, 
            bool fallback = true)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(providerName, nameof(providerName));

            return GetOrNullInternalAsync(name, providerName, providerKey, fallback);
        }

        public virtual async Task<List<FeatureNameValue>> GetAllAsync(
            string providerName, 
            string providerKey, 
            bool fallback = true)
        {
            Check.NotNull(providerName, nameof(providerName));

            var featureDefinitions = FeatureDefinitionManager.GetAll();
            var providers = Enumerable.Reverse(Providers)
                .SkipWhile(c => c.Name != providerName);

            if (!fallback)
            {
                providers = providers.TakeWhile(c => c.Name == providerName);
            }

            var providerList = providers.Reverse().ToList();

            if (!providerList.Any())
            {
                return new List<FeatureNameValue>();
            }

            var featureValues = new Dictionary<string, FeatureNameValue>();

            foreach (var feature in featureDefinitions)
            {
                string value = null;

                foreach (var provider in providerList)
                {
                    var providerValue = await provider.GetOrNullAsync(
                        feature,
                        provider.Name == providerName ? providerKey : null
                    );

                    if (providerValue != null)
                    {
                        value = providerValue;
                    }
                }

                if (value != null)
                {
                    featureValues[feature.Name] = new FeatureNameValue(feature.Name, value);
                }
            }

            return featureValues.Values.ToList();
        }

        public virtual async Task SetAsync(
            string name, 
            string value, 
            string providerName, 
            string providerKey, 
            bool forceToSet = false)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(providerName, nameof(providerName));

            var feature = FeatureDefinitionManager.Get(name);

            var providers = Enumerable
                .Reverse(Providers)
                .SkipWhile(p => p.Name != providerName)
                .ToList();

            if (!providers.Any())
            {
                return;
            }
            
            if (providers.Count > 1 && !forceToSet && value != null)
            {
                var fallbackValue = await GetOrNullInternalAsync(name, providers[1].Name, null);
                if (fallbackValue == value)
                {
                    //Clear the value if it's same as it's fallback value
                    value = null;
                }
            }

            providers = providers
                .TakeWhile(p => p.Name == providerName)
                .ToList(); //Getting list for case of there are more than one provider with same providerName

            if (value == null)
            {
                foreach (var provider in providers)
                {
                    await provider.ClearAsync(feature, providerKey);
                }
            }
            else
            {
                foreach (var provider in providers)
                {
                    await provider.SetAsync(feature, value, providerKey);
                }
            }
        }

        protected virtual async Task<string> GetOrNullInternalAsync(
            string name, 
            string providerName, 
            string providerKey, 
            bool fallback = true) //TODO: Fallback is not used
        {
            var feature = FeatureDefinitionManager.Get(name);
            var providers = Enumerable
                .Reverse(Providers);

            if (providerName != null)
            {
                providers = providers.SkipWhile(c => c.Name != providerName);
            }

            string value = null;
            foreach (var provider in providers)
            {
                value = await provider.GetOrNullAsync(
                    feature,
                    provider.Name == providerName ? providerKey : null
                );

                if (value != null)
                {
                    break;
                }
            }

            return value;
        }
    }
}