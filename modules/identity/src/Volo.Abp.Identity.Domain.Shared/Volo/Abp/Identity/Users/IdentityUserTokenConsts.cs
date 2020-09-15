namespace Volo.Abp.Identity
{
    public static class IdentityUserTokenConsts
    {
        /// <summary>
        /// Default value: 64
        /// </summary>
        public static int MaxLoginProviderLength { get; set; } = 64;

        /// <summary>
        /// Default value: 128
        /// </summary>
        public static int MaxNameLength { get; set; } = 128;
    }
}
