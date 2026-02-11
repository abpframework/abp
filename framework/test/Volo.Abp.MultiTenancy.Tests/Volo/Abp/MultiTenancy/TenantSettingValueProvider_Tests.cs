using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.MultiTenancy;

public class TenantSettingValueProvider_Tests : MultiTenancyTestBase
{
    [Fact]
    public void TenantSettingValueProvider_Should_Add_Correction()
    {
        var options = GetRequiredService<IOptions<AbpSettingOptions>>().Value;

        options.ValueProviders[0].ShouldBe(typeof(DefaultValueSettingValueProvider));
        options.ValueProviders[1].ShouldBe(typeof(ConfigurationSettingValueProvider));
        options.ValueProviders[2].ShouldBe(typeof(GlobalSettingValueProvider));
        options.ValueProviders[3].ShouldBe(typeof(TenantSettingValueProvider));
        options.ValueProviders[4].ShouldBe(typeof(UserSettingValueProvider));
    }
}
