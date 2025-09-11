namespace Volo.Abp.Internal.Telemetry.Constants;

public static class AbpPlatformUrls
{
#if DEBUG
    public const string AbpTelemetryApiUrl = "https://localhost:44393/";
#else
    public const string AbpTelemetryApiUrl = "https://telemetry.abp.io/";
#endif


}