using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Projects;

namespace Volo.Docs.GitHub.Documents.Version
{
    public static class SemanticVersionHelper
    {
        public static List<string> IgnoredVersions = new()
        {
            "master",
            "dev"
        };

        public static List<string> OrderByDescending(List<string> versions)
        {
            return versions.OrderByDescending(v=> SemanticVersion.Parse(NormalizeVersion(v)), new VersionComparer()).ToList();
        }

        public static List<VersionInfo> OrderByDescending(List<VersionInfo> versions)
        {
            return versions.OrderByDescending(v => SemanticVersion.Parse(NormalizeVersion(v.Name)), new VersionComparer()).ToList();
        }

        public static bool IsPreRelease(string version)
        {
            if (IgnoredVersions.Contains(version))
            {
                return false;
            }
            
            return SemanticVersion.Parse(NormalizeVersion(version)).IsPrerelease;
        }

        private static string NormalizeVersion(string version)
        {
            if (IgnoredVersions.Contains(version))
            {
                return version;
            }
            
            version = version.RemovePreFix("v");

            var normalizedVersion = "";

            var versionParts = version.Split("-");

            var firstVersionPartSplitted = versionParts[0].Split(".");

            if (firstVersionPartSplitted.Length > 3)
            {
                normalizedVersion = string.Join(".",firstVersionPartSplitted.Take(3));
            }
            else if (firstVersionPartSplitted.Length < 3)
            {
                normalizedVersion = versionParts[0];
                for (int i = firstVersionPartSplitted.Length; i < 3; i++)
                {
                    normalizedVersion += ".0";
                }
            }
            else
            {
                normalizedVersion = versionParts[0];
            }

            if (versionParts.Length > 1)
            {
                return normalizedVersion + "-" + string.Join("-", versionParts.Skip(1));
            }

            return normalizedVersion;
        }
    }
}
