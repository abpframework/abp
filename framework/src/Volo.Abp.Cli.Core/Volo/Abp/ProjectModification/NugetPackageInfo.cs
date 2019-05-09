namespace Volo.Abp.ProjectModification
{
    public class NugetPackageInfo
    {
        public string Name { get; set; }

        public string ModuleClass { get; set; }

        public int Target { get; set; } //TODO: Enum?

        public NpmPackageInfo DependedNpmPackage { get; set; }
    }
}