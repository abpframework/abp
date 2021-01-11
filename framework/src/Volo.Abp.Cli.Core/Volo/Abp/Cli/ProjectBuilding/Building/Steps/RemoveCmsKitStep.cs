using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class RemoveCmsKitStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            var commonFiles = context.Files.Where(f =>
                f.Name.EndsWith(".csproj") ||
                f.Name.EndsWith("Module.cs") ||
                f.Name.EndsWith("MyProjectNameMigrationsDbContext.cs") ||
                f.Name.EndsWith("MyProjectNameGlobalFeatureConfigurator.cs") ||
                (f.Name.EndsWith(".cshtml") && f.Name.Contains("MyCompanyName.MyProjectName.Web.Public"))
                );

            foreach (var file in commonFiles)
            {
                file.RemoveTemplateCodeIfNot("CMS-KIT");
            }
        }
    }
}
