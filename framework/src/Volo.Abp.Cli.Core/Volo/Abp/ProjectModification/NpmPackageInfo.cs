namespace Volo.Abp.ProjectModification
{
    public class NpmPackageInfo
    {
        public string Name { get; set; }

        public int ApplicationType { get; set; } //TODO: Enum?

        public bool IsPro { get; set; }
    }
}