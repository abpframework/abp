using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Cli.Version;

internal static class CommercialPackages
{
    private static readonly HashSet<string> Packages = new()
    {
        "volo.abp.suite"
        //other PRO packages can be added to this list...
    };

    public static bool IsCommercial(string packageId)
    {
        return Packages.Contains(packageId.ToLowerInvariant()) || IsLeptonXPackage(packageId);
    }

    private static bool IsLeptonXPackage(string packageId)
    {
        return !IsLeptonXLitePackage(packageId) && packageId.Contains("LeptonX");
    }

    private static bool IsLeptonXLitePackage(string packageId)
    {
        return packageId.Contains("LeptonXLite");
    }
}
