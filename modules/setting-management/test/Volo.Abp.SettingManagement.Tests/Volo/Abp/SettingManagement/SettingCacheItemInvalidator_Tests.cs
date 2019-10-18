using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.SettingManagement
{
    public class SettingCacheItemInvalidator_Tests : SettingsTestBase
    {
        private readonly IDistributedCache<SettingCacheItem> _cache;
        private readonly ISettingManagementStore _settingManagementStore;
        private readonly ISettingRepository _settingRepository;
        private readonly SettingTestData _testData;

        public SettingCacheItemInvalidator_Tests()
        {
            _settingManagementStore = GetRequiredService<ISettingManagementStore>();
            _cache = GetRequiredService<IDistributedCache<SettingCacheItem>>();
            _settingRepository = GetRequiredService<ISettingRepository>();
            _testData = GetRequiredService<SettingTestData>();
        }

        [Fact]
        public async Task GetOrNullAsync_Should_Cached()
        {
            // Act
            (await _cache.GetAsync(SettingCacheItem.CalculateCacheKey("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString()))).ShouldBeNull();
            await _settingManagementStore.GetOrNullAsync("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString());
            (await _cache.GetAsync(SettingCacheItem.CalculateCacheKey("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString()))).ShouldNotBeNull();
        }

        [Fact]
        public async Task Cache_Should_Invalidator_WhenSettingChanged()
        {
            // Arrange
            // GetOrNullAsync will cache language.
            await _settingManagementStore.GetOrNullAsync("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString());

            // Act
            var lang = await _settingRepository.FindAsync("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString());
            await _settingRepository.DeleteAsync(lang);

            // Assert
            (await _cache.GetAsync(
                SettingCacheItem.CalculateCacheKey("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString()))).ShouldBeNull();
        }
    }
}
