using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.Features;
using Xunit;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureValueCacheItemInvalidator_Tests : FeatureManagementTestBase<FeatureManagementDomainTestModule>
    {
        private IDistributedCache<FeatureValueCacheItem> _cache;
        private IFeatureValueRepository _featureValueRepository;
        private IFeatureManagementStore _featureManagementStore;

        public FeatureValueCacheItemInvalidator_Tests()
        {
            _cache = GetRequiredService<IDistributedCache<FeatureValueCacheItem>>();
            _featureValueRepository = GetRequiredService<IFeatureValueRepository>();
            _featureManagementStore = GetRequiredService<IFeatureManagementStore>();
        }

        [Fact]
        public async Task Cache_Should_Invalidator_WhenFeatureChanged()
        {
            // Arrange cache feature.
            await _featureManagementStore.GetOrNullAsync(TestFeatureDefinitionProvider.SocialLogins,
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString("N"));

            var feature = await _featureValueRepository.FindAsync(TestFeatureDefinitionProvider.SocialLogins,
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString("N"));

            // Act
            await _featureValueRepository.DeleteAsync(feature);

            // Assert
            (await _cache.GetAsync(FeatureValueCacheItem.CalculateCacheKey(TestFeatureDefinitionProvider.SocialLogins,
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString("N")))).ShouldBeNull();

        }
    }
}
