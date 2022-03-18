using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement;

public class FeatureManager : IFeatureManager, ISingletonDependency
{
    protected IFeatureDefinitionManager FeatureDefinitionManager { get; }
    protected List<IFeatureManagementProvider> Providers => _lazyProviders.Value;
    protected FeatureManagementOptions Options { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    private readonly Lazy<List<IFeatureManagementProvider>> _lazyProviders;

    public FeatureManager(
        IOptions<FeatureManagementOptions> options,
        IServiceProvider serviceProvider,
        IFeatureDefinitionManager featureDefinitionManager,
        IStringLocalizerFactory stringLocalizerFactory)
    {
        FeatureDefinitionManager = featureDefinitionManager;
        StringLocalizerFactory = stringLocalizerFactory;
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

    public virtual async Task<string> GetOrNullAsync(
        string name,
        string providerName,
        string providerKey,
        bool fallback = true)
    {
        Check.NotNull(name, nameof(name));
        Check.NotNull(providerName, nameof(providerName));

        return (await GetOrNullInternalAsync(name, providerName, providerKey, fallback)).Value;
    }

    public virtual async Task<List<FeatureNameValue>> GetAllAsync(
        string providerName,
        string providerKey,
        bool fallback = true)
    {
        return (await GetAllWithProviderAsync(providerName, providerKey, fallback))
            .Select(x => new FeatureNameValue(x.Name, x.Value)).ToList();
    }

    public async Task<FeatureNameValueWithGrantedProvider> GetOrNullWithProviderAsync(string name,
        string providerName, string providerKey, bool fallback = true)
    {
        Check.NotNull(name, nameof(name));
        Check.NotNull(providerName, nameof(providerName));

        return await GetOrNullInternalAsync(name, providerName, providerKey, fallback);
    }

    public async Task<List<FeatureNameValueWithGrantedProvider>> GetAllWithProviderAsync(string providerName,
        string providerKey, bool fallback = true)
    {
        Check.NotNull(providerName, nameof(providerName));

        var featureDefinitions = FeatureDefinitionManager.GetAll();
        var providers = Enumerable.Reverse(Providers).SkipWhile(c => c.Name != providerName);

        if (!fallback)
        {
            providers = providers.TakeWhile(c => c.Name == providerName);
        }

        var providerList = providers.ToList();
        if (!providerList.Any())
        {
            return new List<FeatureNameValueWithGrantedProvider>();
        }

        var featureValues = new Dictionary<string, FeatureNameValueWithGrantedProvider>();

        foreach (var feature in featureDefinitions)
        {
            var featureNameValueWithGrantedProvider = new FeatureNameValueWithGrantedProvider(feature.Name, null);
            foreach (var provider in providerList)
            {
                string pk = null;
                if (provider.Compatible(providerName))
                {
                    pk = providerKey;
                }

                var value = await provider.GetOrNullAsync(feature, pk);
                if (value != null)
                {
                    featureNameValueWithGrantedProvider.Value = value;
                    featureNameValueWithGrantedProvider.Provider = new FeatureValueProviderInfo(provider.Name, pk);
                    break;
                }
            }

            if (featureNameValueWithGrantedProvider.Value != null)
            {
                featureValues[feature.Name] = featureNameValueWithGrantedProvider;
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

        if (feature.ValueType?.Validator.IsValid(value) == false)
        {
            throw new FeatureValueInvalidException(feature.DisplayName.Localize(StringLocalizerFactory));
        }

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
            await using (await providers[0].HandleContextAsync(providerName, providerKey))
            {
                var fallbackValue = await GetOrNullInternalAsync(name, providers[1].Name, null);
                if (fallbackValue.Value == value)
                {
                    //Clear the value if it's same as it's fallback value
                    value = null;
                }
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

    protected virtual async Task<FeatureNameValueWithGrantedProvider> GetOrNullInternalAsync(
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

        var featureNameValueWithGrantedProvider = new FeatureNameValueWithGrantedProvider(name, null);
        foreach (var provider in providers)
        {
            string pk = null;
            if (provider.Compatible(providerName))
            {
                pk = providerKey;
            }

            var value = await provider.GetOrNullAsync(feature, pk);
            if (value != null)
            {
                featureNameValueWithGrantedProvider.Value = value;
                featureNameValueWithGrantedProvider.Provider = new FeatureValueProviderInfo(provider.Name, pk);
                break;
            }
        }

        return featureNameValueWithGrantedProvider;
    }
}
