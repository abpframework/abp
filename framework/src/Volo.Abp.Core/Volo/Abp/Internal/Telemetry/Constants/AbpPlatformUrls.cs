namespace Volo.Abp.Internal.Telemetry.Constants;

public static class AbpPlatformUrls
{
    public static string AbpTelemetryApiUrl
    {
        get => AbpTelemetryApiUrlProduction;
        set => AbpTelemetryApiUrlProduction = value;
    }
    
    public static string AbpTelemetryApiUrlProduction = "https://telemetry.abp.io/";
    public static string AbpTelemetryApiUrlDevelopment = "https://localhost:44393/";
}