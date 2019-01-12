using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Settings
{
    public class SettingManager_Tests : AbpIntegratedTest<AbpSettingsTestModule>
    {
        private readonly ISettingManager _settingManager;

        public SettingManager_Tests()
        {
            _settingManager = GetRequiredService<ISettingManager>();
        }

        [Fact]
        public async Task Should_Get_Null_If_No_Value_Provided_And_No_Default_Value()
        {
            (await _settingManager.GetOrNullAsync(TestSettingNames.TestSettingWithoutDefaultValue))
                .ShouldBeNull();
        }

        [Fact]
        public async Task Should_Get_Default_Value_If_No_Value_Provided_And_There_Is_A_Default_Value()
        {
            (await _settingManager.GetOrNullAsync(TestSettingNames.TestSettingWithDefaultValue))
                .ShouldBe("default-value");
        }

        [Fact]
        public async Task Should_Two_Set_Same_Global_Value_And_Get_Value()
        {
            await _settingManager.SetGlobalAsync(TestSettingNames.TestSettingWithDefaultValue, "Test");

            (await _settingManager.GetOrNullGlobalAsync(TestSettingNames.TestSettingWithDefaultValue)).ShouldBe("Test");

            await _settingManager.SetGlobalAsync(TestSettingNames.TestSettingWithDefaultValue, "Test");

            (await _settingManager.GetOrNullGlobalAsync(TestSettingNames.TestSettingWithDefaultValue)).ShouldBe("Test");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("This is a relatively long text... This is a relatively long text... This is a relatively long text... ")]
        public async Task Should_Set_And_Get_Encrypted_Values(string plainValue)
        {
            (await _settingManager.GetOrNullAsync(TestSettingNames.TestSettingEncrypted))
                .ShouldBeNull();

            await _settingManager.SetAsync(
                TestSettingNames.TestSettingEncrypted,
                plainValue,
                TestSettingValueProvider.ProviderName,
                null
            );

            (await _settingManager.GetOrNullAsync(TestSettingNames.TestSettingEncrypted))
                .ShouldBe(plainValue);
        }

        //TODO: Needs more tests with more advanced scenarios.
    }
}
