using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Volo.Abp.Studio.Helpers;

public static class PathHelper
{
    public static string GetRelativePath([NotNull] string basePath, [NotNull] string targetPath)
    {
        Check.NotNull(basePath, nameof(basePath));
        Check.NotNull(targetPath, nameof(targetPath));

        return new Uri(basePath).MakeRelativeUri(new Uri(targetPath)).ToString();
    }

    public static string EnsureForwardSlash(string path)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return path.Replace("\\", "/");
        }

        return path;
    }

    public static string Normalize(string path)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return path.Replace("/", "\\");
        }

        return path;
    }
}
