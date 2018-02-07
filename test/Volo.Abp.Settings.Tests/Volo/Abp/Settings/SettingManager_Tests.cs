using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Settings
{
    public class SettingManager_Tests : AbpSettingsTestBase
    {
        private readonly ISettingManager _settingManager;

        public SettingManager_Tests()
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
        public async Task Should_Get_From_Store_Without_Entity_Props()
        {
            var value = await _settingManager.GetOrNullAsync("MySetting1");
            value.ShouldBe("42");
        }
    }
}
