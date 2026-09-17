namespace Volo.Abp.Identity;

public static class IdentityUserPasskeyConsts
{
    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxPassKeyNameLength { get; set; } = 256;

    /// <summary>
    /// Default value: 1024
    /// </summary>
    public static int MaxCredentialIdLength { get; set; } = 1024;
}
