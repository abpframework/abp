using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class RemoveEfCoreDependencyFromPublicStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            foreach (var file in context.Files)
            {
                if (file.Name.EndsWith(".cs") || file.Name.EndsWith(".csproj") || file.Name.EndsWith(".json"))
                {
                    file.RemoveTemplateCodeIfNot("EFCORE");
                }
            }
        }
    }
}
