namespace Volo.Abp.IdentityServer
{
    public class SecretConsts
    {
        public const int TypeMaxLength = 250;
        public const int ValueMaxLength = 4000;
        public static int ValueMaxLengthValue { get; set; } = ValueMaxLength;
        public const int DescriptionMaxLength = 2000;
    }
}