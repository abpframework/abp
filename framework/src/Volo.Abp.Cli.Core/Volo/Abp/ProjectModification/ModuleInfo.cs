using System.Collections.Generic;

namespace Volo.Abp.ProjectModification
{
    public class ModuleInfo
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<NugetPackageInfo> NugetPackages { get; set; }

        public List<NpmPackageInfo> NpmPackages { get; set; }
    }
}