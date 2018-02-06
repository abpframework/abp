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
        public void Test1()
        {
            
        }
    }
}
