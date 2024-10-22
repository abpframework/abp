using System.Collections.Generic;

namespace Volo.Abp.Cli.Version;

static internal class CommercialPackages
{
    private readonly static HashSet<string> Packages = new()
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
