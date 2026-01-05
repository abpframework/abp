using System;

namespace Volo.Abp.Internal.Telemetry.Activity.Storage;

static internal class TelemetryPeriod
{
    private const string TestModeEnvironmentVariable = "ABP_TELEMETRY_TEST_MODE";

    static TelemetryPeriod()
    {
        var isTestMode = IsTestModeEnabled();

        InformationSendPeriod = isTestMode
            ? TimeSpan.FromSeconds(15)
            : TimeSpan.FromDays(2);

        ActivitySendPeriod = isTestMode
            ? TimeSpan.FromSeconds(5)
            : TimeSpan.FromDays(1);
    }

    public static TimeSpan ActivitySendPeriod { get; }
    public static TimeSpan InformationSendPeriod { get; }

    public static int MaxActivityRetryCount { get; set; } = 3;
    public static TimeSpan MaxFailedActivityAge { get; set; } = TimeSpan.FromDays(30);

    private static bool IsTestModeEnabled()
    {
        var testModeVariable =
            Environment.GetEnvironmentVariable(TestModeEnvironmentVariable, EnvironmentVariableTarget.User);
        return bool.TryParse(testModeVariable, out var isTestMode) && isTestMode;
    }
}