using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.SettingManagement;

public abstract class DuplicateSettingRecords_Tests<TStartupModule> : SettingManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected ISettingManagementStore SettingManagementStore { get; }
    protected ISettingRepository SettingRepository { get; }

    protected DuplicateSettingRecords_Tests()
    {
        SettingManagementStore = GetRequiredService<ISettingManagementStore>();
        SettingRepository = GetRequiredService<ISettingRepository>();
    }

    [Fact]
    public async Task GetOrNullAsync_Should_Not_Fail_When_Duplicate_Global_Settings_Exist()
    {
        await InsertDuplicateGlobalSettingAsync("MySetting1", "43");

        var value = await SettingManagementStore.GetOrNullAsync("MySetting2", GlobalSettingValueProvider.ProviderName, null);
        value.ShouldBe("default-store-value");

        var duplicatedValue = await SettingManagementStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);
        duplicatedValue.ShouldBe((await SettingRepository.FindAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null)).Value);
    }

    [Fact]
    public async Task GetListAsync_Should_Not_Fail_When_Duplicate_Global_Settings_Exist()
    {
        await InsertDuplicateGlobalSettingAsync("MySetting1", "43");

        var result = await SettingManagementStore.GetListAsync(
            new[] { "MySetting1", "MySetting2" },
            GlobalSettingValueProvider.ProviderName,
            null);

        result.First(x => x.Name == "MySetting1").Value.ShouldBe((await SettingRepository.FindAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null)).Value);
        result.First(x => x.Name == "MySetting2").Value.ShouldBe("default-store-value");
    }

    [Fact]
    public async Task GetListAsync_By_Provider_Should_Return_One_Value_Per_Setting_When_Duplicate_Global_Settings_Exist()
    {
        await InsertDuplicateGlobalSettingAsync("MySetting1", "43");

        var result = await SettingManagementStore.GetListAsync(GlobalSettingValueProvider.ProviderName, null);

        result.Count(x => x.Name == "MySetting1").ShouldBe(1);
        result.Single(x => x.Name == "MySetting1").Value.ShouldBe((await SettingRepository.FindAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null)).Value);
    }

    [Fact]
    public async Task SetAsync_Should_Remove_Duplicate_Global_Settings()
    {
        await InsertDuplicateGlobalSettingAsync("MySetting1", "43");

        await SettingManagementStore.SetAsync("MySetting1", "44", GlobalSettingValueProvider.ProviderName, null);

        var settings = await SettingRepository.GetListAsync(new[] { "MySetting1" }, GlobalSettingValueProvider.ProviderName, null);
        settings.Count.ShouldBe(1);
        settings[0].Value.ShouldBe("44");
        (await SettingManagementStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null)).ShouldBe("44");
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Duplicate_Global_Settings()
    {
        await InsertDuplicateGlobalSettingAsync("MySetting1", "43");

        await SettingManagementStore.DeleteAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);

        (await SettingRepository.GetListAsync(new[] { "MySetting1" }, GlobalSettingValueProvider.ProviderName, null)).ShouldBeEmpty();
        (await SettingManagementStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null)).ShouldBeNull();
    }

    private async Task InsertDuplicateGlobalSettingAsync(string name, string value)
    {
        await SettingRepository.InsertAsync(new Setting(Guid.NewGuid(), name, value, GlobalSettingValueProvider.ProviderName));
        (await SettingRepository.GetListAsync(new[] { name }, GlobalSettingValueProvider.ProviderName, null)).Count.ShouldBe(2);
    }
}
