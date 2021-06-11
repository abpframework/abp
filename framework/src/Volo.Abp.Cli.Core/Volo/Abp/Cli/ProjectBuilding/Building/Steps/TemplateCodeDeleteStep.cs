using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class TemplateCodeDeleteStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            foreach (var file in context.Files)
            {
                if (file.Name.EndsWith(".cs") || file.Name.EndsWith(".csproj") || file.Name.EndsWith(".cshtml") || file.Name.EndsWith(".json"))
                {
                    file.RemoveTemplateCode(context.Symbols);
                    file.RemoveTemplateCodeMarkers();
                }
            }
        }
    }
}
