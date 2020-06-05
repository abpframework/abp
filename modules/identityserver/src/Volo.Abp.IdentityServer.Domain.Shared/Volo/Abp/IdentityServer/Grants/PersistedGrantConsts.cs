namespace Volo.Abp.IdentityServer.Grants
{
    public class PersistedGrantConsts
    {
        public const int KeyMaxLength = 200;
        public const int TypeMaxLength = 50;
        public const int SubjectIdMaxLength = 200;
        public const int ClientIdMaxLength = 200;
        public const int DataMaxLength = 50000;
        public static int DataMaxLengthValue { get; set; }= 50000;
    }
}