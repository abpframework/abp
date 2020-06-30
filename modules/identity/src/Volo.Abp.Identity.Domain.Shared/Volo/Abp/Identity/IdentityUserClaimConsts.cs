namespace Volo.Abp.Identity
{
    public static class IdentityUserClaimConsts
    {
        /// <summary>
        /// Default value: 256
        /// </summary>
        public static int MaxClaimTypeLength { get; set; } = 256;

        /// <summary>
        /// Default value: 1024
        /// </summary>
        public static int MaxClaimValueLength { get; set; } = 1024;
    }
}
