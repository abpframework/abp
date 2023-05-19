using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.SettingManagement;

public class DefaultValueSettingManagementProvider_Tests : SettingsTestBase
{
    private readonly ISettingDefinitionManager _settingDefinitionManager;

    public DefaultValueSettingManagementProvider_Tests()
    {
        _settingDefinitionManager = GetRequiredService<ISettingDefinitionManager>();
    }

    [Fact]
    public async Task GetOrNullAsync()
    {
        var mySetting3 = _settingDefinitionManager.Get("MySetting3");

        var defaultValueSettingManagementProvider = new DefaultValueSettingManagementProvider();
        (await defaultValueSettingManagementProvider
            .GetOrNullAsync(mySetting3, DefaultValueSettingValueProvider.ProviderName)).ShouldBe("123");
    }

    [Fact]
    public async Task SetAsync()
    {
        var mySetting3 = _settingDefinitionManager.Get("MySetting3");

        await Assert.ThrowsAsync<AbpException>(async () => await new DefaultValueSettingManagementProvider().SetAsync(mySetting3, "123",
            DefaultValueSettingValueProvider.ProviderName));
    }

    [Fact]
    public async Task ClearAsync()
    {
        var mySetting3 = _settingDefinitionManager.Get("MySetting3");

        await Assert.ThrowsAsync<AbpException>(async () =>
            await new DefaultValueSettingManagementProvider().ClearAsync(mySetting3,
                DefaultValueSettingValueProvider.ProviderName));
    }
}
