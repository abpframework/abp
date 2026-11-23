using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Docs.Projects;

namespace Volo.Docs.Admin.Projects
{
    public class CreateProjectDto
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Format { get; set; }

        public string DefaultDocumentName { get; set; }

        public string NavigationDocumentName { get; set; }

        public string ParametersDocumentName { get; set; }

        public string MinimumVersion { get; set; }

        public string MainWebsiteUrl { get; set; }

        public string LatestVersionBranchName { get; set; }

        public string DocumentStoreType { get; set; }

        [DisableAuditing]
        public Dictionary<string, object> ExtraProperties { get; set; }
    }
}
