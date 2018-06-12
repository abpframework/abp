namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public static class BundleContributorListExtensions
    {
        public static void AddFiles(this BundleContributorCollection contributors, params string[] files)
        {
            contributors.Add(new SimpleBundleContributor(files));
        }
    }
}