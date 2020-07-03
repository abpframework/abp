namespace Volo.Abp.IdentityServer.Devices
{
    public class DeviceFlowCodesConsts
    {
        public const int DeviceCodeMaxLength = 200;
        public const int UserCodeMaxLength = 200;
        public const int SubjectIdMaxLength = 200;
        public const int ClientIdMaxLength = 200;
        public const int DataMaxLength = 50000;
        public static int DataMaxLengthValue { get; set; } = DataMaxLength;
    }
}
