using System;

namespace Volo.Abp.Cli.Licensing
{
    public class DeveloperApiKeyResult
    {
        public bool HasActiveLicense { get; set; }
        public string OrganizationName { get; set; }
        public string ApiKey { get; set; }
        public DateTime LicenseEndTime { get; set; }
        public string LicenseCode { get; set; }
    }
}