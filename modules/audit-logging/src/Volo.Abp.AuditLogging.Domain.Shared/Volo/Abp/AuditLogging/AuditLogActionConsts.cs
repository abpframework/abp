namespace Volo.Abp.AuditLogging;

public class AuditLogActionConsts
{
    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxServiceNameLength { get; set; } = 256;

    /// <summary>
    /// Default value: 128
    /// </summary>
    public static int MaxMethodNameLength { get; set; } = 128;

    /// <summary>
    /// Default value: 2000
    /// </summary>
    public static int MaxParametersLength { get; set; } = 2000;
}
