using System;

namespace Volo.Abp.Cli.Licensing
{
    public class DeveloperApiKeyResult
    {
        public bool HasActiveLicense { get; set; }
        public string OrganizationName { get; set; }
        public string ApiKey { get; set; }
        public DateTime? LicenseEndTime { get; set; }
        public bool CanDownloadSourceCode { get; set; }
        public string LicenseCode { get; set; }
        public string ErrorMessage { get; set; }
        public LicenseErrorType? ErrorType { get; set; }
        public LicenseType LicenseType { get; set; }

        public enum LicenseErrorType
        {
            NotAuthenticated = 1,
            NotMemberOfAnOrganization = 2,
            NoActiveLicense = 3,
            NotDeveloperOfTheOrganization = 4
        }
    }
}