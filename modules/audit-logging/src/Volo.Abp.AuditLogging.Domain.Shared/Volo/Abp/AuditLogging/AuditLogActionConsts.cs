namespace Volo.Abp.AuditLogging
{
    public class AuditLogActionConsts
    {
        public static int MaxServiceNameLength { get; set; } = 256;

        public static int MaxMethodNameLength { get; set; } = 128;

        public static int MaxParametersLength { get; set; } = 2000;
    }
}
