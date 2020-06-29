namespace Volo.Abp.Identity
{
    public static class IdentityUserLoginConsts
    {
        public static int MaxLoginProviderLength { get; set; } = 64;
        public static int MaxProviderKeyLength { get; set; } = 196;
        public static int MaxProviderDisplayNameLength { get; set; } = 128;
    }
}
