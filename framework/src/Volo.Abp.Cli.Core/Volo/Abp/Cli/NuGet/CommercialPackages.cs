using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Cli.NuGet;

internal static class CommercialPackages
{
    private readonly static HashSet<string> Packages = new()
    {
        "volo.abp.suite"
        //other PRO packages can be added to this list...
    };

    public static bool IsCommercial(string packageId)
    {
        return Packages.Contains(packageId.ToLowerInvariant());
    }
}
