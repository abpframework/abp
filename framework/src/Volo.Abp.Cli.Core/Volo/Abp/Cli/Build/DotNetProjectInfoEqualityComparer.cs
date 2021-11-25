using System.Collections.Generic;

namespace Volo.Abp.Cli.Build;

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
