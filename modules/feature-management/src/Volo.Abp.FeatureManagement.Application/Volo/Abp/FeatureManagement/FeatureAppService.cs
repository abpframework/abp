using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
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

        public virtual async Task<FeatureListDto> GetAsync([NotNull] string providerName, [NotNull] string providerKey)
        {
            await CheckProviderPolicy(providerName);

            var featureDefinitions = FeatureDefinitionManager.GetAll();
            var features = new List<FeatureDto>();

            foreach (var featureDefinition in featureDefinitions)
            {
                features.Add(new FeatureDto
                {
                    Name = featureDefinition.Name,
                    DisplayName = featureDefinition.DisplayName?.Localize(StringLocalizerFactory),
                    ValueType = featureDefinition.ValueType,
                    Description = featureDefinition.Description?.Localize(StringLocalizerFactory),
                    ParentName = featureDefinition.Parent?.Name,
                    Value = await FeatureManager.GetOrNullAsync(featureDefinition.Name, providerName, providerKey)
                });
            }

            SetFeatureDepth(features, providerName, providerKey);

            return new FeatureListDto { Features = features };
        }

        public virtual async Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdateFeaturesDto input)
        {
            await CheckProviderPolicy(providerName);

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

        protected virtual async Task CheckProviderPolicy(string providerName)
        {
            var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
            if (policyName.IsNullOrEmpty())
            {
                throw new AbpException($"No policy defined to get/set permissions for the provider '{policyName}'. Use {nameof(FeatureManagementOptions)} to map the policy.");
            }

            await AuthorizationService.CheckAsync(policyName);
        }
    }
}
