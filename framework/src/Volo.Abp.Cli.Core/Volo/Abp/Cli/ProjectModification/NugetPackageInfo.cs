namespace Volo.Abp.Cli.ProjectModification
{
    public class NugetPackageInfo
    {
        public string Name { get; set; }

        public string ModuleClass { get; set; }

        public NuGetPackageTarget Target { get; set; }

        public NpmPackageInfo DependedNpmPackage { get; set; }
    }
}