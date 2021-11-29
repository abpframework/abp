using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.ProjectModification
{
    public class ModuleInfo
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string EfCoreConfigureMethodName { get; set; }

        public string DocumentationLinks { get; set; }

        public List<NugetPackageInfo> NugetPackages { get; set; }

        public List<NpmPackageInfo> NpmPackages { get; set; }

        public string InstallationCompleteMessage { get; set; }

        public string GetFirstDocumentationLinkOrNull()
        {
            if (string.IsNullOrWhiteSpace(DocumentationLinks))
            {
                return null;
            }

            var docs = DocumentationLinks.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return docs.Any() ?
                docs.First() :
                null;
        }
    }
}
