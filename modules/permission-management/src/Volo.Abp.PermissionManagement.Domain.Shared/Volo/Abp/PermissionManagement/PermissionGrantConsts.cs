namespace Volo.Abp.PermissionManagement;

public static class PermissionGrantConsts
{
    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxProviderNameLength { get; set; } = 64;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxProviderKeyLength { get; set; } = 64;

    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxResourceNameLength { get; set; } = 256;

    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxResourceKeyLength { get; set; } = 256;
}
