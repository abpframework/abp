using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class RemoveGlobalFeaturesPackageStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            var commonFiles = context.Files.Where(f =>
                f.Name.EndsWith(".csproj") ||
                f.Name.EndsWith(".cs") ||
                f.Name.EndsWith(".cshtml"));

            foreach (var file in commonFiles)
            {
                file.RemoveTemplateCodeIf("CMS-KIT");
            }
        }
    }
}
