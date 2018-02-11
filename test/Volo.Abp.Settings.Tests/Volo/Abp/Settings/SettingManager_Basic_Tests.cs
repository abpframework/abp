using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Settings
{
    public class SettingManager_Basic_Tests : SettingsTestBase
    {
        private readonly ISettingManager _settingManager;

        public SettingManager_Basic_Tests()
        {
            _settingManager = GetRequiredService<ISettingManager>();
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Try_To_Get_An_Undefined_Setting()
        {
            await Assert.ThrowsAsync<AbpException>(
                async () => await _settingManager.GetOrNullAsync("UndefinedSetting")
            );
        }

        [Fact]
        public async Task Should_Get_Default_Value_If_Not_Set_In_Store()
        {
            var value = await _settingManager.GetOrNullAsync("SettingNotSetInStore");
            value.ShouldBe("default-value");
        }

        [Fact]
        public async Task Should_Get_Base_Store_Value()
        {
            (await _settingManager.GetOrNullAsync("MySetting1")).ShouldBe("42");
        }

        [Fact]
        public async Task Should_Get_All_Base_Store_Values()
        {
            var settingValues = await _settingManager.GetAllAsync();
            settingValues.ShouldContain(sv => sv.Name == "MySetting1" && sv.Value == "42");
            settingValues.ShouldContain(sv => sv.Name == "MySetting2" && sv.Value == "default-store-value");
            settingValues.ShouldContain(sv => sv.Name == "SettingNotSetInStore" && sv.Value == "default-value");
        }

        [Fact]
        public async Task Should_Set_Global_Value()
        {
            await _settingManager.SetGlobalAsync("MySetting1", "43");

            (await _settingManager.GetOrNullGlobalAsync("MySetting1")).ShouldBe("43");
            (await _settingManager.GetOrNullAsync("MySetting1")).ShouldBe("43");
        }
    }
}
