using System.Collections.Generic;
using Shouldly;
using TimeZoneConverter;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Timing;

public class TimeZoneHelper_Tests : AbpIntegratedTest<AbpTimingTestModule>
{
    [Fact]
    public void GetTimezones_Should_Filter_Invalid_Timezones()
    {
        var validTimeZoneId = "UTC";
        var invalidTimeZoneId = "Invalid/Zone";

        var timezones = new List<NameValue>
        {
            new(invalidTimeZoneId, invalidTimeZoneId),
            new(validTimeZoneId, validTimeZoneId)
        };

        var result = TimeZoneHelper.GetTimezones(timezones);

        result.Count.ShouldBe(1);

        var expectedTimeZoneInfo = TZConvert.GetTimeZoneInfo(validTimeZoneId);
        var expectedName = $"{validTimeZoneId} ({TimeZoneHelper.GetTimezoneOffset(expectedTimeZoneInfo)})";

        result[0].Name.ShouldBe(expectedName);
        result[0].Value.ShouldBe(validTimeZoneId);
    }

    [Fact]
    public void TryCreateNameValueWithOffset_Should_Return_Null_For_Invalid_Timezone()
    {
        TimeZoneHelper.TryCreateNameValueWithOffset(new NameValue("Invalid/Zone", "Invalid/Zone")).ShouldBeNull();
    }
}
