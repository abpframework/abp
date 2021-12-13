using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement
{
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

            foreach (var group in FeatureDefinitionManager.GetGroups())
            {
                var groupDto = new FeatureGroupDto
                {
                    Name = group.Name,
                    DisplayName = group.DisplayName.Localize(StringLocalizerFactory),
                    Features = new List<FeatureDto>()
                };

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
                    groupDto.Features.Add(new FeatureDto
                    {
                        Name = featureDefinition.Name,
                        DisplayName = featureDefinition.DisplayName?.Localize(StringLocalizerFactory),
                        ValueType = featureDefinition.ValueType,
                        Description = featureDefinition.Description?.Localize(StringLocalizerFactory),
                        ParentName = featureDefinition.Parent?.Name,
                        Value = feature.Value,
                        Provider = new FeatureProviderDto
                        {
                            Name = feature.Provider?.Name,
                            Key = feature.Provider?.Key
                        }
                    });
                }

                SetFeatureDepth(groupDto.Features, providerName, providerKey);

                if (groupDto.Features.Any())
                {
                    result.Groups.Add(groupDto);
                }
            }

            return result;
        }

        public virtual async Task UpdateAsync([NotNull] string providerName, string providerKey, UpdateFeaturesDto input)
        {
            await CheckProviderPolicy(providerName, providerKey);

            foreach (var feature in input.Features)
            {
                await FeatureManager.SetAsync(feature.Name, feature.Value, providerName, providerKey);
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
            if (providerName == TenantFeatureValueProvider.ProviderName && CurrentTenant.Id == null && providerKey == null )
            {
                policyName = "FeatureManagement.ManageHostFeatures";
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
    }
}
