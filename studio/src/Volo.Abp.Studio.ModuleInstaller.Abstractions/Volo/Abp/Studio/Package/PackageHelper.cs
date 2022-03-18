using System;
using System.IO;
using JetBrains.Annotations;

namespace Volo.Abp.Studio.Packages;

public static class PackageHelper
{
    public static string GetNameFromPath([NotNull] string path)
    {
        Check.NotNullOrWhiteSpace(path, nameof(path));

        return Path
            .GetFileName(path)
            .RemovePostFix(StringComparison.OrdinalIgnoreCase, PackageConsts.FileExtension);
    }
}
