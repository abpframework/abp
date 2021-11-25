namespace Volo.Abp.IdentityServer.Devices;

public class DeviceFlowCodesConsts
{
    public static int DeviceCodeMaxLength { get; set; } = 200;

    public static int UserCodeMaxLength { get; set; } = 200;

    public static int SubjectIdMaxLength { get; set; } = 200;

    public static int SessionIdMaxLength { get; set; } = 100;

    public static int DescriptionMaxLength { get; set; } = 200;

    public static int ClientIdMaxLength { get; set; } = 200;

    public static int DataMaxLength { get; set; } = 50000;
}
