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
        public async Task Test1()
        {
            var value = await _settingManager.GetOrNullAsync("MySetting1");
            value.ShouldBe("42");
        }
    }
}
