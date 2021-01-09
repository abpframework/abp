using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class RemoveEfCoreDependencyFromPublicStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            context.Files.FirstOrDefault(f => f.Name.EndsWith("MyCompanyName.MyProjectName.Web.Public.csproj"))?.RemoveTemplateCodeIfNot("EFCORE");
            context.Files.FirstOrDefault(f => f.Name.EndsWith("MyProjectNameWebPublicModule.cs"))?.RemoveTemplateCodeIfNot("EFCORE");
        }
    }
}
