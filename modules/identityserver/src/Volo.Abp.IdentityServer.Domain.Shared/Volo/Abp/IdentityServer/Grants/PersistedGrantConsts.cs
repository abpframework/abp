namespace Volo.Abp.IdentityServer.Grants;

public class PersistedGrantConsts
{
    /// <summary>
    /// Default value: 200
    /// </summary>
    public static int KeyMaxLength { get; set; } = 200;

    /// <summary>
    /// Default value: 50
    /// </summary>
    public static int TypeMaxLength { get; set; } = 50;

    /// <summary>
    /// Default value: 200
    /// </summary>
    public static int SubjectIdMaxLength { get; set; } = 200;

    /// <summary>
    /// Default value: 100
    /// </summary>
    public static int SessionIdMaxLength { get; set; } = 100;

    /// <summary>
    /// Default value: 200
    /// </summary>
    public static int ClientIdMaxLength { get; set; } = 200;

    /// <summary>
    /// Default value: 200
    /// </summary>
    public static int DescriptionMaxLength { get; set; } = 200;

    /// <summary>
    /// Default value: 50000
    /// </summary>
    public static int DataMaxLength { get; set; } = 50000;

    /// <summary>
    /// Default value: 50000
    /// </summary>
    public static int DataMaxLengthValue { get; set; } = 50000;
}
