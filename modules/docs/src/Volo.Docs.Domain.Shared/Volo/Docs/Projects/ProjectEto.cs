using System;

namespace Volo.Docs.Projects
{
    [Serializable]
    public class ProjectEto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Format { get; set; }

        public string DefaultDocumentName { get; set; }

        public string NavigationDocumentName { get; set; }

        public string ParametersDocumentName { get; set; }

        public string MinimumVersion { get; set; }

        public string DocumentStoreType { get; set; }

        public string MainWebsiteUrl { get; set; }

        public string LatestVersionBranchName { get; set; }
    }
}