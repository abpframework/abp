namespace Volo.Abp.Identity;

public class IdentitySessionConsts
{
    public static int MaxSessionIdLength { get; set; } = 128;

    public static int MaxDeviceLength { get; set; } = 64;

    public static int MaxDeviceInfoLength { get; set; } = 64;

    public static int MaxClientIdLength { get; set; } = 64;

    public static int MaxIpAddressesLength { get; set; } = 2048;
}
