using Volo.Abp.SolutionTemplating.Files;

namespace Volo.Abp.SolutionTemplating.Building.Steps
{
    public class TemplateCodeDeleteStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            foreach (var file in context.Files)
            {
                if (file.Name.EndsWith(".cs")) //TODO: Why only cs!
                {
                    file.RemoveTemplateCode();
                    file.RemoveTemplateCodeMarkers();
                }
            }
        }
    }
}