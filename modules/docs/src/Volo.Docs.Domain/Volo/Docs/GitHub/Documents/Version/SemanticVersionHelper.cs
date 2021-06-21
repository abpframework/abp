using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Projects;

namespace Volo.Docs.GitHub.Documents.Version
{
    public class SemanticVersionHelper : IVersionHelper, ITransientDependency
    {
        public List<string> OrderByDescending(List<string> versions)
        {
            return versions.OrderByDescending(v=> SemanticVersion.Parse(NormalizeVersion(v)), new VersionComparer()).ToList();
        }

        public List<VersionInfo> OrderByDescending(List<VersionInfo> versions)
        {
            return versions.OrderByDescending(v => SemanticVersion.Parse(NormalizeVersion(v.Name)), new VersionComparer()).ToList();
        }

        public bool IsPreRelease(string version)
        {
            return SemanticVersion.Parse(NormalizeVersion(version)).IsPrerelease;
        }

        private string NormalizeVersion(string version)
        {
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
