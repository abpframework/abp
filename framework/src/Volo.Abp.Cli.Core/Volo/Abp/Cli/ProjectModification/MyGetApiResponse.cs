using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.ProjectModification;

public class MyGetApiResponse
{
    public string _date { get; set; }

    public List<MyGetPackage> Packages { get; set; }

    /// <summary>
    /// Returns the latest version of the given package, or <c>null</c> if the response doesn't contain it.
    /// <see cref="Packages"/> and <see cref="MyGetPackage.Versions"/> can be <c>null</c> when the endpoint
    /// answers with an unexpected payload, so both are treated as "no version available".
    /// </summary>
    public string GetLatestVersionOrNull(string packageId)
    {
        return Packages?
            .FirstOrDefault(p => p?.Id == packageId)?
            .Versions?
            .LastOrDefault();
    }
}
