using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement;

[Authorize]
public class FeatureAppService : FeatureManagementAppServiceBase, IFeatureAppService
{
    protected FeatureManagementOptions Options { get; }
    protected IFeatureManager FeatureManager { get; }
    protected IFeatureDefinitionManager FeatureDefinitionManager { get; }

    public FeatureAppService(IFeatureManager featureManager,
        IFeatureDefinitionManager featureDefinitionManager,
        IOptions<FeatureManagementOptions> options)
    {
        FeatureManager = featureManager;
        FeatureDefinitionManager = featureDefinitionManager;
        Options = options.Value;
    }

    public virtual async Task<GetFeatureListResultDto> GetAsync([NotNull] string providerName, string providerKey)
    {
        await CheckProviderPolicy(providerName, providerKey);

        var result = new GetFeatureListResultDto
        {
            Groups = new List<FeatureGroupDto>()
        };

        foreach (var group in await FeatureDefinitionManager.GetGroupsAsync())
        {
            var groupDto = CreateFeatureGroupDto(group);

            foreach (var featureDefinition in group.GetFeaturesWithChildren())
            {
                if (providerName == TenantFeatureValueProvider.ProviderName &&
                    CurrentTenant.Id == null &&
                    providerKey == null &&
                    !featureDefinition.IsAvailableToHost)
                {
                    continue;
                }

                var feature = await FeatureManager.GetOrNullWithProviderAsync(featureDefinition.Name, providerName, providerKey);
                groupDto.Features.Add(CreateFeatureDto(feature, featureDefinition));
            }

            SetFeatureDepth(groupDto.Features, providerName, providerKey);

            if (groupDto.Features.Any())
            {
                result.Groups.Add(groupDto);
            }
        }

        return result;
    }

    private FeatureGroupDto CreateFeatureGroupDto(FeatureGroupDefinition groupDefinition)
    {
        return new FeatureGroupDto
        {
            Name = groupDefinition.Name,
            DisplayName = groupDefinition.DisplayName?.Localize(StringLocalizerFactory),
            Features = new List<FeatureDto>()
        };
    }

    private FeatureDto CreateFeatureDto(FeatureNameValueWithGrantedProvider featureNameValueWithGrantedProvider, FeatureDefinition featureDefinition)
    {
        return new FeatureDto
        {
            Name = featureDefinition.Name,
            DisplayName = featureDefinition.DisplayName?.Localize(StringLocalizerFactory),
            Description = featureDefinition.Description?.Localize(StringLocalizerFactory),

            ValueType = featureDefinition.ValueType,

            ParentName = featureDefinition.Parent?.Name,
            Value = featureNameValueWithGrantedProvider.Value,
            Provider = new FeatureProviderDto
            {
                Name = featureNameValueWithGrantedProvider.Provider?.Name,
                Key = featureNameValueWithGrantedProvider.Provider?.Key
            }
        };
    }

    public virtual async Task UpdateAsync([NotNull] string providerName, string providerKey, UpdateFeaturesDto input)
    {
        await CheckProviderPolicy(providerName, providerKey);

        var inputFeatureNames = input.Features.Select(f => f.Name).ToHashSet();
        var featureMap = input.Features.ToDictionary(f => f.Name);
        var features = new Dictionary<UpdateFeatureDto, List<UpdateFeatureDto>>();
        var processed = new HashSet<string>();

        foreach (var feature in input.Features)
        {
            if (!processed.Add(feature.Name))
            {
                continue;
            }

            var featureDefinition = await FeatureDefinitionManager.GetAsync(feature.Name);
            var validChildren = new List<UpdateFeatureDto>();

            foreach (var childFeature in featureDefinition.Children)
            {
                if (inputFeatureNames.Contains(childFeature.Name) &&
                    featureMap.TryGetValue(childFeature.Name, out var childDto) &&
                    processed.Add(childFeature.Name))
                {
                    validChildren.Add(childDto);
                }
            }

            features[feature] = validChildren;
        }

        foreach (var feature in features)
        {
            var forceToSet = false;
            foreach (var childFeature in feature.Value)
            {
                await FeatureManager.SetAsync(childFeature.Name, childFeature.Value, providerName, providerKey);
                var value = await FeatureManager.GetOrNullWithProviderAsync(childFeature.Name, providerName, providerKey);
                if (value.Provider.Name == providerName && value.Provider.Key == providerKey)
                {
                    forceToSet = true;
                }
            }

            await FeatureManager.SetAsync(feature.Key.Name, feature.Key.Value, providerName, providerKey, forceToSet: forceToSet);
        }
    }

    protected virtual void SetFeatureDepth(List<FeatureDto> features, string providerName, string providerKey,
        FeatureDto parentFeature = null, int depth = 0)
    {
        foreach (var feature in features)
        {
            if ((parentFeature == null && feature.ParentName == null) || (parentFeature != null && parentFeature.Name == feature.ParentName))
            {
                feature.Depth = depth;
                SetFeatureDepth(features, providerName, providerKey, feature, depth + 1);
            }
        }
    }

    protected virtual async Task CheckProviderPolicy(string providerName, string providerKey)
    {
        string policyName;
        if (providerName == TenantFeatureValueProvider.ProviderName && CurrentTenant.Id == null && providerKey == null)
        {
            policyName = FeatureManagementPermissions.ManageHostFeatures;
        }
        else
        {
            policyName = Options.ProviderPolicies.GetOrDefault(providerName);
            if (policyName.IsNullOrEmpty())
            {
                throw new AbpException($"No policy defined to get/set permissions for the provider '{providerName}'. Use {nameof(FeatureManagementOptions)} to map the policy.");
            }
        }

        await AuthorizationService.CheckAsync(policyName);
    }

    public virtual async Task DeleteAsync([NotNull] string providerName, string providerKey)
    {
        await CheckProviderPolicy(providerName, providerKey);
        await FeatureManager.DeleteAsync(providerName, providerKey);
    }
}
