namespace Volo.Abp.Users
{
    public class AbpUserConsts
    {
        public static int MaxUserNameLength { get; set; } = 256;
        
        public static int MaxNameLength { get; set; } = 64;
        
        public static int MaxSurnameLength { get; set; } = 64;

        public static int MaxEmailLength { get; set; } = 256;

        public static int MaxPhoneNumberLength { get; set; } = 16;
    }
}