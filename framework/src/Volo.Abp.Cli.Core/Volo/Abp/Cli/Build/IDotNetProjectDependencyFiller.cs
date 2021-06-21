using System.Collections.Generic;

namespace Volo.Abp.Cli.Build
{
    public interface IDotNetProjectDependencyFiller
    {
        void Fill(List<DotNetProjectInfo> projects);
    }
}
