namespace Volo.Abp.Cli.ProjectBuilding.Building
{
    public class ModuleInfo
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public string DocumentUrl { get; set; }

        public string DisplayName { get; set; }

        public string ShortDescription { get; set; }

        public bool IsPro { get; set; }

        public bool EfCoreSupport { get; set; }

        public bool MongoDBSupport { get; set; }

        public bool AngularUi { get; set; }

        public bool MvcUi { get; set; }

        public bool BlazorUi { get; set; }

        public bool IsFreeToActiveLicenseOwners { get; set; }
    }
}
