namespace Volo.Abp.IdentityServer.Grants
{
    public class PersistedGrantConsts
    {
        public const int KeyMaxLength = 200;
        public const int TypeMaxLength = 50;
        public const int SubjectIdMaxLength = 200;
        public const int ClientIdMaxLength = 200;

        // 50000 chosen to be explicit to allow enough size to avoid truncation, yet stay beneath the MySql row size limit of ~65K
        // apparently anything over 4K converts to nvarchar(max) on SqlServer
        public const int DataMaxLength = 50000;
    }
}