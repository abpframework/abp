namespace Volo.Abp.Users;

public class AbpUserConsts
{
    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxUserNameLength { get; set; } = 256;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxNameLength { get; set; } = 64;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxSurnameLength { get; set; } = 64;

    /// <summary>
    /// Default value: 256
    /// </summary>
    public static int MaxEmailLength { get; set; } = 256;

    /// <summary>
    /// Default value: 16
    /// </summary>
    public static int MaxPhoneNumberLength { get; set; } = 16;
}
