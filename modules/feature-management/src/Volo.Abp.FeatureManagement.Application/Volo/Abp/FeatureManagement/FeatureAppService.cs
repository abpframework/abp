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

        private readonly IFeatureManager _featureManager;
        private readonly IFeatureDefinitionManager _featureDefinitionManager;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public FeatureAppService(IFeatureManager featureManager,
            IFeatureDefinitionManager featureDefinitionManager,
            IStringLocalizerFactory stringLocalizerFactory,
            IOptions<FeatureManagementOptions> options)
        {
            _featureManager = featureManager;
            _featureDefinitionManager = featureDefinitionManager;
            _stringLocalizerFactory = stringLocalizerFactory;
            Options = options.Value;
        }

        public async Task<FeatureListDto> GetAsync([NotNull] string providerName, [NotNull] string providerKey)
        {
            await CheckProviderPolicy(providerName);

            var featureDefinitions = _featureDefinitionManager.GetAll();
            var features = new List<FeatureDto>();

            foreach (var featureDefinition in featureDefinitions)
            {
                features.Add(new FeatureDto
                {
                    Name = featureDefinition.Name,
                    DisplayName = featureDefinition.DisplayName?.Localize(_stringLocalizerFactory),
                    ValueType = featureDefinition.ValueType,
                    Description = featureDefinition.Description?.Localize(_stringLocalizerFactory),
                    ParentName = featureDefinition.Parent?.Name,
                    Value = await _featureManager.GetOrNullAsync(featureDefinition.Name, providerName, providerKey)
                });
            }

            SetFeatureDepth(features, providerName, providerKey);

            return new FeatureListDto { Features = features };
        }

        public async Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdateFeaturesDto input)
        {
            await CheckProviderPolicy(providerName);

            foreach (var feature in input.Features)
            {
                await _featureManager.SetAsync(feature.Name, feature.Value, providerName, providerKey);
            }
        }

        private void SetFeatureDepth(List<FeatureDto> features, string providerName, string providerKey,
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
