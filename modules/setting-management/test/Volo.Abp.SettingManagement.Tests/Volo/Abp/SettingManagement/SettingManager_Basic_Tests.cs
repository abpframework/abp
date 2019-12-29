using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.SettingManagement
{
    public class SettingManager_Basic_Tests : SettingsTestBase
    {
        private readonly ISettingManager _settingManager;
        private readonly ISettingProvider _settingProvider;

        public SettingManager_Basic_Tests()
        {
            _settingManager = GetRequiredService<ISettingManager>();
            _settingProvider = GetRequiredService<ISettingProvider>();
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Try_To_Get_An_Undefined_Setting()
        {
            await Assert.ThrowsAsync<AbpException>(
                async () => await _settingProvider.GetOrNullAsync("UndefinedSetting")
.ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task Should_Get_Default_Value_If_Not_Set_In_Store()
        {
            var value = await _settingProvider.GetOrNullAsync("SettingNotSetInStore").ConfigureAwait(false);
            value.ShouldBe("default-value");
        }

        [Fact]
        public async Task Should_Get_Base_Store_Value()
        {
            (await _settingProvider.GetOrNullAsync("MySetting1").ConfigureAwait(false)).ShouldBe("42");
        }

        [Fact]
        public async Task Should_Get_All_Base_Store_Values()
        {
            var settingValues = await _settingProvider.GetAllAsync().ConfigureAwait(false);
            settingValues.ShouldContain(sv => sv.Name == "MySetting1" && sv.Value == "42");
            settingValues.ShouldContain(sv => sv.Name == "MySetting2" && sv.Value == "default-store-value");
            settingValues.ShouldContain(sv => sv.Name == "SettingNotSetInStore" && sv.Value == "default-value");
        }

        [Fact]
        public async Task Should_Set_Global_Value()
        {
            await _settingManager.SetGlobalAsync("MySetting1", "43").ConfigureAwait(false);

            (await _settingManager.GetOrNullGlobalAsync("MySetting1").ConfigureAwait(false)).ShouldBe("43");
            (await _settingProvider.GetOrNullAsync("MySetting1").ConfigureAwait(false)).ShouldBe("43");
        }
    }
}
