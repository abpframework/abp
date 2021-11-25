namespace Volo.Abp.Identity;

public class IdentityClaimTypeConsts
{
    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxNameLength { get; set; } = 256;

    /// <summary>
    /// Default value: 512
    /// </summary>
    public static int MaxRegexLength { get; set; } = 512;

    /// <summary>
    /// Default value: 128
    /// </summary>
    public static int MaxRegexDescriptionLength { get; set; } = 128;

    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxDescriptionLength { get; set; } = 256;
}
