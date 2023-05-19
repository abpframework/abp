using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.SettingManagement;

public class SettingCacheItemInvalidator_Tests : SettingsTestBase
{
    private readonly IDistributedCache<SettingCacheItem> _cache;
    private readonly ISettingManagementStore _settingManagementStore;
    private readonly ISettingRepository _settingRepository;
    private readonly SettingTestData _testData;
    private readonly ICurrentTenant _currentTenant;

    public SettingCacheItemInvalidator_Tests()
    {
        _settingManagementStore = GetRequiredService<ISettingManagementStore>();
        _cache = GetRequiredService<IDistributedCache<SettingCacheItem>>();
        _settingRepository = GetRequiredService<ISettingRepository>();
        _testData = GetRequiredService<SettingTestData>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
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

    [Fact]
    public async Task Cache_Should_Invalidator_WhenSettingChanged_Between_Tenant_And_Host()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            // GetOrNullAsync will cache language.
            await _settingManagementStore
                .GetOrNullAsync("MySetting2", GlobalSettingValueProvider.ProviderName, null)
                ;
        }

        using (_currentTenant.Change(null))
        {
            // SetAsync will make cache invalid.
            await _settingManagementStore
                .SetAsync("MySetting2", "MySetting2Value", GlobalSettingValueProvider.ProviderName, null);
        }

        using (_currentTenant.Change(tenantId))
        {
            // Assert
            (await _cache.GetAsync(
                    SettingCacheItem.CalculateCacheKey("MySetting2", GlobalSettingValueProvider.ProviderName, null))
                ).ShouldBeNull();
        }
    }
}
