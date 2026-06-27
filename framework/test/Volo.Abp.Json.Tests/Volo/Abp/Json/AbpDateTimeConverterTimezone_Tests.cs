using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.Json;

/// <summary>
/// Regression tests for the warning
///   "Could not convert DateTime with unspecified Kind using timezone '...'."
/// logged by <c>AbpDateTimeConverterBase.Normalize</c>.
///
/// When <see cref="AbpClockOptions.Kind"/> is <see cref="DateTimeKind.Utc"/> the converter treats
/// an <see cref="DateTimeKind.Unspecified"/> value as local time in the current user's timezone and
/// converts it to UTC via <c>new DateTimeOffset(value, offset).UtcDateTime</c>. For a placeholder
/// such as <see cref="DateTime.MinValue"/> a positive offset (e.g. Asia/Shanghai = +08:00) pushes
/// the value before <see cref="DateTime.MinValue"/> and throws <see cref="ArgumentOutOfRangeException"/>,
/// which used to be swallowed and logged as a warning on every serialization. The converter now
/// skips the timezone conversion for the <see cref="DateTime.MinValue"/>/<see cref="DateTime.MaxValue"/>
/// sentinel values, so no warning is emitted while the serialized output stays unchanged.
/// </summary>
public class AbpDateTimeConverterTimezone_Tests : AbpJsonSystemTextJsonTestBase
{
    private const string WarningFragment = "Could not convert DateTime with unspecified Kind";

    private static readonly CapturingLoggerProvider LogCapture = new();

    private readonly IJsonSerializer _jsonSerializer;
    private readonly ICurrentTimezoneProvider _currentTimezoneProvider;

    public AbpDateTimeConverterTimezone_Tests()
    {
        _jsonSerializer = GetRequiredService<IJsonSerializer>();
        _currentTimezoneProvider = GetRequiredService<ICurrentTimezoneProvider>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        // The warning only happens with a UTC clock, where Unspecified values get converted to UTC.
        services.Configure<AbpClockOptions>(options => options.Kind = DateTimeKind.Utc);

        LogCapture.Clear();
        services.AddSingleton<ILoggerProvider>(LogCapture);

        base.AfterAddApplication(services);
    }

    private sealed class FileModel
    {
        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; set; }
    }

    [Theory]
    [InlineData("Asia/Shanghai")]   // +08:00
    [InlineData("Europe/Brussels")] // +01:00 / +02:00
    public void Should_Not_Warn_When_Serializing_MinValue_Under_Positive_Offset_Timezone(string timeZoneId)
    {
        _currentTimezoneProvider.TimeZone = timeZoneId;
        LogCapture.Clear();

        DateTime.MinValue.Kind.ShouldBe(DateTimeKind.Unspecified);

        var json = _jsonSerializer.Serialize(new FileModel
        {
            DateModified = DateTime.MinValue,
            DateCreated = DateTime.MinValue
        });

        // The placeholder is serialized unchanged and no warning is logged.
        json.ShouldContain("0001-01-01");
        LogCapture.Warnings.ShouldNotContain(m => m.Contains(WarningFragment));
    }

    [Fact]
    public void Should_Not_Warn_When_Serializing_Value_Near_MinValue_Under_Positive_Offset_Timezone()
    {
        // Not exactly DateTime.MinValue: any value within the offset distance of the lower bound
        // overflows the same way, so the converter must absorb it rather than warn.
        _currentTimezoneProvider.TimeZone = "Asia/Shanghai"; // +08:00
        LogCapture.Clear();

        var nearMin = DateTime.MinValue.AddHours(3); // 0001-01-01T03:00:00 - 08:00 underflows

        _jsonSerializer.Serialize(new FileModel
        {
            DateModified = nearMin,
            DateCreated = nearMin
        });

        LogCapture.Warnings.ShouldNotContain(m => m.Contains(WarningFragment));
    }

    [Fact]
    public void Should_Not_Warn_When_Serializing_MaxValue_Under_Negative_Offset_Timezone()
    {
        // The symmetric case: a negative offset would push MaxValue past DateTime.MaxValue.
        _currentTimezoneProvider.TimeZone = "America/New_York";
        LogCapture.Clear();

        var json = _jsonSerializer.Serialize(new FileModel
        {
            DateModified = DateTime.MaxValue,
            DateCreated = DateTime.MaxValue
        });

        json.ShouldContain("9999-12-31");
        LogCapture.Warnings.ShouldNotContain(m => m.Contains(WarningFragment));
    }

    [Fact]
    public void Should_Not_Warn_When_Serializing_Real_Utc_Timestamp_Under_Positive_Offset_Timezone()
    {
        _currentTimezoneProvider.TimeZone = "Asia/Shanghai";
        LogCapture.Clear();

        var utc = new DateTime(2026, 6, 27, 10, 28, 7, DateTimeKind.Utc);

        var json = _jsonSerializer.Serialize(new FileModel
        {
            DateModified = utc,
            DateCreated = utc
        });

        json.ShouldContain("2026-06-27T10:28:07Z");
        LogCapture.Warnings.ShouldNotContain(m => m.Contains(WarningFragment));
    }

    [Fact]
    public void Should_Still_Convert_Real_Unspecified_Timestamp_To_Utc_Under_Positive_Offset_Timezone()
    {
        // A genuine (non-sentinel) Unspecified value must still be converted to UTC using the offset.
        _currentTimezoneProvider.TimeZone = "Asia/Shanghai"; // +08:00
        LogCapture.Clear();

        var unspecified = new DateTime(2026, 6, 27, 18, 0, 0, DateTimeKind.Unspecified);

        var json = _jsonSerializer.Serialize(new FileModel
        {
            DateModified = unspecified,
            DateCreated = unspecified
        });

        // 18:00 in +08:00 == 10:00 UTC.
        json.ShouldContain("2026-06-27T10:00:00Z");
        LogCapture.Warnings.ShouldNotContain(m => m.Contains(WarningFragment));
    }

    private sealed class CapturingLoggerProvider : ILoggerProvider
    {
        public ConcurrentQueue<string> Warnings { get; } = new();

        public void Clear()
        {
            Warnings.Clear();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CapturingLogger(Warnings);
        }

        public void Dispose()
        {
        }

        private sealed class CapturingLogger : ILogger
        {
            private readonly ConcurrentQueue<string> _sink;

            public CapturingLogger(ConcurrentQueue<string> sink)
            {
                _sink = sink;
            }

            public IDisposable BeginScope<TState>(TState state) where TState : notnull
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= LogLevel.Warning;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter)
            {
                if (logLevel >= LogLevel.Warning)
                {
                    _sink.Enqueue(formatter(state, exception));
                }
            }
        }
    }
}
