using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Settings;

public class SettingProvider_Tests : AbpIntegratedTest<AbpSettingsTestModule>
{
    private readonly ISettingProvider _settingProvider;

    public SettingProvider_Tests()
    {
        _settingProvider = GetRequiredService<ISettingProvider>();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public async Task Should_Get_Null_If_No_Value_Provided_And_No_Default_Value()
    {
        (await _settingProvider.GetOrNullAsync(TestSettingNames.TestSettingWithoutDefaultValue))
            .ShouldBeNull();
    }

    [Fact]
    public async Task Should_Get_Default_Value_If_No_Value_Provided_And_There_Is_A_Default_Value()
    {
        (await _settingProvider.GetOrNullAsync(TestSettingNames.TestSettingWithDefaultValue))
            .ShouldBe("default-value");
    }
}
