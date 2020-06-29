namespace Volo.Abp.AuditLogging
{
    public class EntityPropertyChangeConsts
    {
        public static int MaxNewValueLength { get; set; } = 512;

        public static int MaxOriginalValueLength { get; set; } = 512;

        public static int MaxPropertyNameLength { get; set; } = 128;

        public static int MaxPropertyTypeFullNameLength { get; set; } = 64;
    }
}
