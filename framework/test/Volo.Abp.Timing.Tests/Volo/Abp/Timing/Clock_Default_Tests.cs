using System;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Timing;

public class Clock_Default_Tests : AbpIntegratedTest<AbpTimingTestModule>
{
    private readonly IClock _clock;
    private readonly ICurrentTimezoneProvider _currentTimezoneProvider;

    public Clock_Default_Tests()
    {
        _clock = GetRequiredService<IClock>();
        _currentTimezoneProvider = GetRequiredService<ICurrentTimezoneProvider>();
    }

    [Fact]
    public void ConvertTo_Test()
    {
        using(_currentTimezoneProvider.Change("Europe/Istanbul"))
        {
            var dateTime = new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Utc);
            var convertedDateTime = _clock.ConvertToUserTime(dateTime);
            convertedDateTime.Kind.ShouldBe(DateTimeKind.Utc);
            convertedDateTime.ToString("O").ShouldBe("2025-03-01T05:30:00.0000000Z");
        }

        using(_currentTimezoneProvider.Change("Europe/Istanbul"))
        {
            var dateTime = new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Local);
            var convertedDateTime = _clock.ConvertToUserTime(dateTime);
            convertedDateTime.Kind.ShouldBe(DateTimeKind.Local);
            convertedDateTime.ToString("O").ShouldBe(dateTime.ToString("O"));
        }

        using(_currentTimezoneProvider.Change(null))
        {
            var dateTime = new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Local);
            var convertedDateTime = _clock.ConvertToUserTime(dateTime);
            convertedDateTime.Kind.ShouldBe(DateTimeKind.Local);
            convertedDateTime.ToString("O").ShouldBe(dateTime.ToString("O"));
        }
    }

    [Fact]
    public void ConvertTo_DateTimeOffset_Test()
    {
        using(_currentTimezoneProvider.Change("Europe/Istanbul"))
        {
            var dateTimeOffset = new DateTimeOffset(new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Utc), TimeSpan.Zero);
            var convertedDateTimeOffset = _clock.ConvertToUserTime(dateTimeOffset);
            convertedDateTimeOffset.Offset.ShouldBe(TimeSpan.Zero);
            convertedDateTimeOffset.ShouldBe(dateTimeOffset);
        }

        using(_currentTimezoneProvider.Change("Europe/Istanbul"))
        {
            var dateTimeOffset = new DateTimeOffset(new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Unspecified), TimeSpan.Zero);
            var convertedDateTimeOffset = _clock.ConvertToUserTime(dateTimeOffset);
            convertedDateTimeOffset.Offset.ShouldBe(TimeSpan.Zero);
            convertedDateTimeOffset.ShouldBe(dateTimeOffset);
        }

        using(_currentTimezoneProvider.Change("Europe/Istanbul"))
        {
            var dateTimeOffset = new DateTimeOffset(new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Unspecified), TimeSpan.FromHours(3));
            var convertedDateTimeOffset = _clock.ConvertToUserTime(dateTimeOffset);
            convertedDateTimeOffset.Offset.ShouldBe(TimeSpan.FromHours(3));
            convertedDateTimeOffset.ShouldBe(dateTimeOffset);
        }

        using(_currentTimezoneProvider.Change("Europe/Istanbul"))
        {
            var dateTimeOffset = new DateTimeOffset(new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Unspecified), TimeSpan.FromHours(8));
            var convertedDateTimeOffset = _clock.ConvertToUserTime(dateTimeOffset);
            convertedDateTimeOffset.Offset.ShouldBe(TimeSpan.FromHours(8));
            convertedDateTimeOffset.ShouldBe(dateTimeOffset);
        }
    }

    [Fact]
    public void ConvertFrom_Test()
    {
        using(_currentTimezoneProvider.Change("Europe/Istanbul"))
        {
            var dateTime = new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Unspecified);
            var convertedDateTime = _clock.ConvertToUtc(dateTime);
            convertedDateTime.Kind.ShouldBe(DateTimeKind.Unspecified);
            convertedDateTime.ShouldBe(dateTime);
        }

        using(_currentTimezoneProvider.Change("Europe/Istanbul"))
        {
            var dateTime = new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Local);
            var convertedDateTime = _clock.ConvertToUtc(dateTime);
            convertedDateTime.Kind.ShouldBe(DateTimeKind.Local);
            convertedDateTime.ShouldBe(dateTime);
        }

        using(_currentTimezoneProvider.Change("Europe/Istanbul"))
        {
            var dateTime = new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Utc);
            var convertedDateTime = _clock.ConvertToUtc(dateTime);
            convertedDateTime.Kind.ShouldBe(DateTimeKind.Utc);
            convertedDateTime.ShouldBe(dateTime);
        }
    }
}
