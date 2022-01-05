namespace Volo.Abp.Studio.Modules;

public static class ModuleConsts
{
    public const string FileExtension = ".abpmdl.json";
    public const string InstallerPackagePostfix = ".Installer";
    public const string SourceCorePackagePostfix = ".SourceCode";
    public const string Packages = "packages";

    public static class Layers //TODO: Moving to PackageTypes
    {
        public const string Domain = "lib.domain";
        public const string DomainShared = "lib.domain.shared";
        public const string Application = "lib.application";
        public const string ApplicationContracts = "lib.application.contracts";
    }
}
