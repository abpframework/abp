using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.SettingManagement;

public class SettingManager_Basic_Tests : SettingsTestBase
{
    private readonly ISettingManager _settingManager;
    private readonly ISettingProvider _settingProvider;
    private readonly ISettingManagementStore _settingManagementStore;

    public SettingManager_Basic_Tests()
    {
        _settingManager = GetRequiredService<ISettingManager>();
        _settingProvider = GetRequiredService<ISettingProvider>();
        _settingManagementStore = GetRequiredService<ISettingManagementStore>();
    }

    [Fact]
    public async Task Should_Return_Null_When_Try_To_Get_An_Undefined_Setting()
    {
        var value = await _settingProvider.GetOrNullAsync("UndefinedSetting");
        value.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Get_Default_Value_If_Not_Set_In_Store()
    {
        var value = await _settingProvider.GetOrNullAsync("SettingNotSetInStore");
        value.ShouldBe("default-value");
    }

    [Fact]
    public async Task Should_Get_Base_Store_Value()
    {
        (await _settingProvider.GetOrNullAsync("MySetting1")).ShouldBe("42");
    }

    [Fact]
    public async Task Should_Get_All_Base_Store_Values()
    {
        var settingValues = await _settingProvider.GetAllAsync();
        settingValues.ShouldContain(sv => sv.Name == "MySetting1" && sv.Value == "42");
        settingValues.ShouldContain(sv => sv.Name == "MySetting2" && sv.Value == "default-store-value");
        settingValues.ShouldContain(sv => sv.Name == "SettingNotSetInStore" && sv.Value == "default-value");
    }

    [Fact]
    public async Task Should_Get_All_By_Names_Base_Store_Values()
    {
        var settingValues = await _settingProvider.GetAllAsync(new []{ "MySetting1", "MySetting2" });
        settingValues.Count.ShouldBe(2);
        settingValues.ShouldContain(sv => sv.Name == "MySetting1" && sv.Value == "42");
        settingValues.ShouldContain(sv => sv.Name == "MySetting2" && sv.Value == "default-store-value");
    }

    [Fact]
    public async Task Should_Set_Global_Value()
    {
        await _settingManager.SetGlobalAsync("MySetting1", "43");

        (await _settingManager.GetOrNullGlobalAsync("MySetting1")).ShouldBe("43");
        (await _settingProvider.GetOrNullAsync("MySetting1")).ShouldBe("43");
    }

    [Fact]
    public async Task Set_Should_Throw_Exception_If_Provider_Not_Found()
    {
        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _settingManager.SetAsync("MySetting1", "43", "UndefinedProvider", "Test");
        });

        exception.Message.ShouldBe("Unknown setting value provider: UndefinedProvider");
    }

    [Fact]
    public async Task Set_Should_Throw_Exception_If_Provider_Not_In_Providers()
    {
        var exception = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _settingManager.SetGlobalAsync(TestSettingDefinitionProvider.UserOnlySetting, "value");
        });

        exception.Message.ShouldContain(TestSettingDefinitionProvider.UserOnlySetting);
        exception.Message.ShouldContain(GlobalSettingValueProvider.ProviderName);
    }

    [Fact]
    public async Task Set_Should_Allow_Setting_For_Provider_In_Providers()
    {
        var userId = Guid.NewGuid();

        await _settingManager.SetForUserAsync(userId, TestSettingDefinitionProvider.UserOnlySetting, "value");

        (await _settingManager.GetOrNullForUserAsync(
            TestSettingDefinitionProvider.UserOnlySetting,
            userId)).ShouldBe("value");
    }

    [Fact]
    public async Task GetAllAsync_Should_Not_Return_Settings_With_Disallowed_Provider()
    {
        var userId = Guid.NewGuid();
        await _settingManager.SetForUserAsync(userId, TestSettingDefinitionProvider.UserOnlySetting, "user-value");

        var globalSettings = await _settingManager.GetAllGlobalAsync();
        globalSettings.ShouldNotContain(x => x.Name == TestSettingDefinitionProvider.UserOnlySetting);

        var userSettings = await _settingManager.GetAllForUserAsync(userId);
        userSettings.ShouldContain(x =>
            x.Name == TestSettingDefinitionProvider.UserOnlySetting && x.Value == "user-value");
    }

    [Fact]
    public async Task GetOrNullForGlobal_Should_Not_Read_From_Disallowed_Provider()
    {
        await _settingManagementStore.SetAsync(
            TestSettingDefinitionProvider.UserOnlySetting,
            "stale",
            GlobalSettingValueProvider.ProviderName,
            null);

        (await _settingManager.GetOrNullGlobalAsync(
            TestSettingDefinitionProvider.UserOnlySetting)).ShouldBeNull();
    }

    [Fact]
    public async Task GetAllForUser_Should_Inherit_Setting_From_Allowed_Upstream_Provider()
    {
        await _settingManager.SetGlobalAsync(
            TestSettingDefinitionProvider.GlobalOnlySetting,
            "global-value");

        var userSettings = await _settingManager.GetAllForUserAsync(Guid.NewGuid());

        userSettings.ShouldContain(x =>
            x.Name == TestSettingDefinitionProvider.GlobalOnlySetting && x.Value == "global-value");
    }

    [Fact]
    public async Task GetOrNullForUser_Should_Inherit_Value_From_Allowed_Upstream_Provider()
    {
        await _settingManager.SetGlobalAsync(
            TestSettingDefinitionProvider.GlobalOnlySetting,
            "global-value");

        (await _settingManager.GetOrNullForUserAsync(
            TestSettingDefinitionProvider.GlobalOnlySetting,
            Guid.NewGuid())).ShouldBe("global-value");
    }

    [Fact]
    public async Task SetForUser_Should_Not_Be_Cleared_By_Stale_Disallowed_Provider_Fallback()
    {
        var userId = Guid.NewGuid();

        await _settingManagementStore.SetAsync(
            TestSettingDefinitionProvider.UserOnlySetting,
            "user-value",
            GlobalSettingValueProvider.ProviderName,
            null);

        await _settingManager.SetForUserAsync(
            userId,
            TestSettingDefinitionProvider.UserOnlySetting,
            "user-value");

        (await _settingManager.GetOrNullForUserAsync(
            TestSettingDefinitionProvider.UserOnlySetting,
            userId)).ShouldBe("user-value");
    }
}
