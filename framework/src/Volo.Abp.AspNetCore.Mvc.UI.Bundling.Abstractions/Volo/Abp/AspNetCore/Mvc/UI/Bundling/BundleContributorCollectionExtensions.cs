using System;
using System.Linq;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public static class BundleContributorCollectionExtensions
{
    public static void AddFiles(this BundleContributorCollection contributors, params string[] files)
    {
        contributors.Add(new BundleFileContributor(files));
    }

    public static void AddFiles(this BundleContributorCollection contributors, params BundleFile[] files)
    {
        contributors.Add(new BundleFileContributor(files));
    }

    public static void AddExternalFiles(this BundleContributorCollection contributors, params BundleFile[] files)
    {
        contributors.Add(new BundleFileContributor(files));
    }

    public static void RemoveFile(this BundleContributorCollection contributors, params string[] files)
    {
        contributors.RemoveBundleFile(files.ToArray());
    }

    public static void RemoveFile(this BundleContributorCollection contributors, Func<string, bool> predicate)
    {
        contributors.RemoveBundleFile(predicate);
    }
}
