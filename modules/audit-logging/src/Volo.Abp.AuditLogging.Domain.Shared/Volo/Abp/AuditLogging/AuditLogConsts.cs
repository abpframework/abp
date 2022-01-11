namespace Volo.Abp.AuditLogging;

public static class AuditLogConsts
{
    /// <summary>
    /// Default value: 96
    /// </summary>
    public static int MaxApplicationNameLength { get; set; } = 96;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxClientIpAddressLength { get; set; } = 64;

    /// <summary>
    /// Default value: 128
    /// </summary>
    public static int MaxClientNameLength { get; set; } = 128;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxClientIdLength { get; set; } = 64;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxCorrelationIdLength { get; set; } = 64;

    /// <summary>
    /// Default value: 512
    /// </summary>
    public static int MaxBrowserInfoLength { get; set; } = 512;

    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxCommentsLength { get; set; } = 256;

    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxUrlLength { get; set; } = 256;

    /// <summary>
    /// Default value: 16
    /// </summary>
    public static int MaxHttpMethodLength { get; set; } = 16;

    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxUserNameLength { get; set; } = 256;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxTenantNameLength { get; set; } = 64;
}
