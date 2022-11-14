namespace Volo.Abp.TenantManagement;

public static class TenantConsts
{
    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxNameLength { get; set; } = 64;

    public const int MaxPasswordLength = 128; 
    
    public const int AdminEmailAddress = 256; 
}
