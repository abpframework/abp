using System.Collections.Generic;

namespace Volo.Abp.Cli.Build
{
    public interface IDotNetProjectBuilder
    {
        List<string> BuildProjects(List<DotNetProjectInfo> projects, string arguments);
        
        void BuildSolution(string slnPath, string arguments);
    }
}
