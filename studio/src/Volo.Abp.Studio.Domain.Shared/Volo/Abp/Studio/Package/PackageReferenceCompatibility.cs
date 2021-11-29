namespace Volo.Abp.Studio.Packages.Referencing
{
    public enum PackageReferenceCompatibility
    {
        Unknown,
        DDD_Compatible,
        Compatible,
        TestProject,
        IndirectlyReferenced,
        DirectlyReferenced,
        CircularReference
    }
}
