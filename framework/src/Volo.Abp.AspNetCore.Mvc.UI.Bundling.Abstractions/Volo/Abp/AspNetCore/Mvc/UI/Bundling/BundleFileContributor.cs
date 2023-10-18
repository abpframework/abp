using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class BundleFileContributor : BundleContributor
{
    public List<BundleFile> Files { get; }

    public BundleFileContributor(params BundleFile[] files)
    {
        Files = new List<BundleFile>();
        Files.AddRange(files);
    }

    public BundleFileContributor(params string[] files)
    {
        Files = new List<BundleFile>();
        Files.AddRange(files.Select(file => new BundleFile(file)));
    }

    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        foreach (var file in Files)
        {
            context.Files.AddIfNotContains(x => x.FileName.Equals(file.FileName, StringComparison.OrdinalIgnoreCase), () => file);
        }
    }
}
