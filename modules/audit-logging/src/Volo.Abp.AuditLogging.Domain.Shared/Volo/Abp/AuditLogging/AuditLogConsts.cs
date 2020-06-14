﻿namespace Volo.Abp.AuditLogging
{
    public static class AuditLogConsts
    {
        public const int MaxApplicationNameLength = 96;

        public const int MaxClientIpAddressLength = 64;

        public const int MaxClientNameLength = 128;

        public const int MaxClientIdLength = 64;

        public const int MaxCorrelationIdLength = 64;

        public const int MaxBrowserInfoLength = 512;

        public const int MaxExceptionsLength = 4000;

        //TODO: Replace with MaxExceptionsLength in v3.0
        public static int MaxExceptionsLengthValue { get; set; } = MaxExceptionsLength; 

        public const int MaxCommentsLength = 256;

        public const int MaxUrlLength = 256;

        public const int MaxHttpMethodLength = 16;

        public const int MaxUserNameLength = 256;

        public const int MaxTenantNameLength = 64;
    }
}
