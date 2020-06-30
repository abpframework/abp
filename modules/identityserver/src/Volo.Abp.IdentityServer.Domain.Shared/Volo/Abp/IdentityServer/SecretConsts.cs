namespace Volo.Abp.IdentityServer
{
    public class SecretConsts
    {
        /// <summary>
        /// Default value: 250
        /// </summary>
        public static int TypeMaxLength { get; set; } = 250;
        
        /// <summary>
        /// Default value: 4000
        /// </summary>
        public static int ValueMaxLength { get; set; } = 4000;
        
        public static int ValueMaxLengthValue { get; set; } = ValueMaxLength;
        
        /// <summary>
        /// Default value: 2000
        /// </summary>
        public static int DescriptionMaxLength { get; set; } = 2000;
    }
}