using System;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Timing;

public class TimezoneProviderExtensions_Tests : AbpIntegratedTest<AbpTimingTestModule>
{
    private readonly ITimezoneProvider _timezoneProvider;

    public TimezoneProviderExtensions_Tests()
    {
        _timezoneProvider = GetRequiredService<ITimezoneProvider>();
    }

    [Theory]
    [InlineData("Asia/Shanghai")]   // +08:00
    [InlineData("Europe/Brussels")] // +01:00 / +02:00
    public void Should_Keep_MinValue_Unchanged_Under_Positive_Offset(string timeZoneId)
    {
        // A positive offset would push DateTime.MinValue below the supported range; keep it as-is.
        var result = _timezoneProvider.ConvertUnspecifiedToUtc(DateTime.MinValue, timeZoneId);

        result.ShouldBe(DateTime.MinValue);
        result.Kind.ShouldBe(DateTimeKind.Unspecified);
    }

    [Fact]
    public void Should_Keep_Value_Near_MinValue_Unchanged_Under_Positive_Offset()
    {
        var nearMin = DateTime.MinValue.AddHours(3); // 0001-01-01T03:00:00 - 08:00 underflows

        var result = _timezoneProvider.ConvertUnspecifiedToUtc(nearMin, "Asia/Shanghai");

        result.ShouldBe(nearMin);
        result.Kind.ShouldBe(DateTimeKind.Unspecified);
    }

    [Fact]
    public void Should_Keep_MaxValue_Unchanged_Under_Negative_Offset()
    {
        var result = _timezoneProvider.ConvertUnspecifiedToUtc(DateTime.MaxValue, "America/New_York");

        result.ShouldBe(DateTime.MaxValue);
        result.Kind.ShouldBe(DateTimeKind.Unspecified);
    }

    [Fact]
    public void Should_Convert_Unspecified_Value_To_Utc_Using_Offset()
    {
        var unspecified = new DateTime(2026, 6, 27, 18, 0, 0, DateTimeKind.Unspecified);

        var result = _timezoneProvider.ConvertUnspecifiedToUtc(unspecified, "Asia/Shanghai"); // +08:00

        // 18:00 in +08:00 == 10:00 UTC.
        result.ShouldBe(new DateTime(2026, 6, 27, 10, 0, 0, DateTimeKind.Utc));
        result.Kind.ShouldBe(DateTimeKind.Utc);
    }
}
