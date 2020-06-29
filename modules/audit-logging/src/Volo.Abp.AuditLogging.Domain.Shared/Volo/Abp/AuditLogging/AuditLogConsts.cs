namespace Volo.Abp.AuditLogging
{
    public static class AuditLogConsts
    {
        public static int MaxApplicationNameLength { get; set; } = 96;

        public static int MaxClientIpAddressLength { get; set; } = 64;

        public static int MaxClientNameLength { get; set; } = 128;

        public static int MaxClientIdLength { get; set; } = 64;

        public static int MaxCorrelationIdLength { get; set; } = 64;

        public static int MaxBrowserInfoLength { get; set; } = 512;

        public static int MaxExceptionsLength { get; set; } = 4000;

        //TODO: Replace with MaxExceptionsLength in v3.0
        public static int MaxExceptionsLengthValue { get; set; } = MaxExceptionsLength; 

        public static int MaxCommentsLength { get; set; } = 256;

        public static int MaxUrlLength { get; set; } = 256;

        public static int MaxHttpMethodLength { get; set; } = 16;

        public static int MaxUserNameLength { get; set; } = 256;

        public static int MaxTenantNameLength { get; set; } = 64;
    }
}
