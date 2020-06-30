namespace Volo.Abp.Identity
{
    public static class IdentityUserLoginConsts
    {
        /// <summary>
        /// Default value: 64
        /// </summary>
        public static int MaxLoginProviderLength { get; set; } = 64;
        
        /// <summary>
        /// Default value: 196
        /// </summary>
        public static int MaxProviderKeyLength { get; set; } = 196;
        
        /// <summary>
        /// Default value: 128
        /// </summary>
        public static int MaxProviderDisplayNameLength { get; set; } = 128;
    }
}
