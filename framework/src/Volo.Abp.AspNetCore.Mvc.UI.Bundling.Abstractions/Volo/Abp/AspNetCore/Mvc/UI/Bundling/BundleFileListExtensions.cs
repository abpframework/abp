using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

/// <summary>
/// This class is used to compatible with old code.
/// </summary>
public static class BundleFileListExtensions
{
    public static void Add(this List<BundleFile> bundleFiles, params string[] files)
    {
        bundleFiles.AddRange(files.Select(file => new BundleFile(file)));
    }

    public static void AddRange(this List<BundleFile> bundleFiles, params string[] files)
    {
        bundleFiles.AddRange(files.Select(file => new BundleFile(file)));
    }

    public static void AddIfNotContains(this List<BundleFile> bundleFiles, params string[] files)
    {
        foreach (var file in files)
        {
            if (!bundleFiles.Any(x => x.File.Equals(file, StringComparison.OrdinalIgnoreCase)))
            {
                bundleFiles.Add(new BundleFile(file));
            }
        }
    }
}
