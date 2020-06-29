namespace Volo.Abp.Identity
{
    public class IdentityClaimTypeConsts
    {
        public static int MaxNameLength { get; set; } = 256;
        public static int MaxRegexLength { get; set; } = 512;
        public static int MaxRegexDescriptionLength { get; set; } = 128;
        public static int MaxDescriptionLength { get; set; } = 256;
        public static int MaxConcurrencyStampLength { get; set; } = 256;
    }
}
