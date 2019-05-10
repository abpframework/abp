namespace Volo.Abp.ProjectModification
{
    public class NugetPackageInfo
    {
        public string Name { get; set; }

        public string ModuleClass { get; set; }

        public NugetPackageTarget Target { get; set; }

        public NpmPackageInfo DependedNpmPackage { get; set; }
    }
}