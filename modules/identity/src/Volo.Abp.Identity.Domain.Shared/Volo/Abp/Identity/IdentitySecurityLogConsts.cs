namespace Volo.Abp.Identity;

public class IdentitySecurityLogConsts
{
    /// <summary>
    /// Default value: 96
    /// </summary>
    public static int MaxApplicationNameLength { get; set; } = 96;

    /// <summary>
    /// Default value: 96
    /// </summary>
    public static int MaxIdentityLength { get; set; } = 96;

    /// <summary>
    /// Default value: 96
    /// </summary>
    public static int MaxActionLength { get; set; } = 96;


    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxUserNameLength { get; set; } = 256;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxTenantNameLength { get; set; } = 64;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxClientIpAddressLength { get; set; } = 64;

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

}
