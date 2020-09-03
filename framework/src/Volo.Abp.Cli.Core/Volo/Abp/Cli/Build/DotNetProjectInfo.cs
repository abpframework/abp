using System;
using System.Collections.Generic;

namespace Volo.Abp.Cli.Build
{
    [Serializable]
    public class DotNetProjectInfo
    {
        public string RepositoryName { get; set; }

        public string CsProjPath { get; set; }

        public List<DotNetProjectInfo> Dependencies { get; set; }

        public DotNetProjectInfo(string repositoryName, string csProjPath)
        {
            RepositoryName = repositoryName;
            CsProjPath = csProjPath;
            Dependencies = new List<DotNetProjectInfo>();
        }
    }
    
    public class DotNetProjectInfoEqualityComparer : EqualityComparer<DotNetProjectInfo>
    {
        public override bool Equals(DotNetProjectInfo x, DotNetProjectInfo y)
        {
            return (x == null && y == null) || (x != null && y != null && x.CsProjPath == y.CsProjPath);
        }

        public override int GetHashCode(DotNetProjectInfo obj)
        {
            return obj == null ? 0 : obj.CsProjPath.GetHashCode();
        }
    }
}
