using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Projects;

namespace Volo.Docs.Version
{
    public class SemanticVersionHelper : IVersionHelper, ITransientDependency
    {
        public List<string> OrderByDescending(List<string> versions)
        {
            return versions.OrderByDescending(v=> SemanticVersion.Parse(NormalizeVersion(v)), new VersionComparer()).ToList();
        }

        public List<VersionInfoDto> OrderByDescending(List<VersionInfoDto> versions)
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

            if (versionParts[0].Split(".").Length > 3)
            {
                normalizedVersion = string.Join(".",versionParts[0].Split(".").Take(3));
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
