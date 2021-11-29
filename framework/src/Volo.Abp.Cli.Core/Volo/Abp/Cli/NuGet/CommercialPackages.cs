using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Cli.NuGet
{
    internal static class CommercialPackages
    {
        private static readonly HashSet<string> _packages = new()
        {
            "volo.abp.suite"
            //other PRO packages can be added to this list...
        };

        public static bool IsCommercial(string packageId)
        {
            return _packages.Contains(packageId.ToLowerInvariant());
        }
    }
}
