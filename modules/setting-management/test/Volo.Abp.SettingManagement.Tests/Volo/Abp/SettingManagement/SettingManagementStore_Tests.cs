using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.SettingManagement
{
    public class SettingManagementStore_Tests : SettingsTestBase
    {
        private readonly ISettingManagementStore _settingManagementStore;
        private readonly ISettingRepository _settingRepository;
        private readonly SettingTestData _testData;

        public SettingManagementStore_Tests()
        {
            _settingManagementStore = GetRequiredService<ISettingManagementStore>();
            _settingRepository = GetRequiredService<ISettingRepository>();
            _testData = GetRequiredService<SettingTestData>();
        }

        [Fact]
        public async Task GetOrNull_NotExist_Should_Be_Null()
        {
            var value = await _settingManagementStore.GetOrNullAsync("notExistName", "notExistProviderName",
                "notExistProviderKey");

            value.ShouldBeNull();
        }

        [Fact]
        public async Task GetOrNullAsync()
        {
            var value = await _settingManagementStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);

            value.ShouldNotBeNull();
            value.ShouldBe("42");
        }

        [Fact]
        public async Task SetAsync()
        {
            var setting = await _settingRepository.FindAsync(_testData.SettingId);
            setting.Value.ShouldBe("42");

            await _settingManagementStore.SetAsync("MySetting1", "43", GlobalSettingValueProvider.ProviderName, null);

            (await _settingRepository.FindAsync(_testData.SettingId)).Value.ShouldBe("43");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            (await _settingRepository.FindAsync(_testData.SettingId)).ShouldNotBeNull();

            await _settingManagementStore.DeleteAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);

            (await _settingRepository.FindAsync(_testData.SettingId)).ShouldBeNull();
        }

    }
}
