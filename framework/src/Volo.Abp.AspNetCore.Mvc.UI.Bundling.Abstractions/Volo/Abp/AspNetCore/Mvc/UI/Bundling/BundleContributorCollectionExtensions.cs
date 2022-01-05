namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public static class BundleContributorCollectionExtensions
{
    public static void AddFiles(this BundleContributorCollection contributors, params string[] files)
    {
        contributors.Add(new BundleFileContributor(files));
    }
}
