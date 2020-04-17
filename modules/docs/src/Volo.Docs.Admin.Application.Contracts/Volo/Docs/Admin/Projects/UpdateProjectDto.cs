using System.Collections.Generic;

namespace Volo.Docs.Admin.Projects
{
    public class UpdateProjectDto
    {
        public string Name { get; set; }

        public string Format { get; set; }

        public string DefaultDocumentName { get; set; }

        public string NavigationDocumentName { get; set; }

        public string ParametersDocumentName { get; set; }

        public string MinimumVersion { get; set; }

        public string MainWebsiteUrl { get; set; }

        public string LatestVersionBranchName { get; set; }

        public Dictionary<string, object> ExtraProperties { get; set; }
    }
}