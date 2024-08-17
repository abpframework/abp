using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class AppNoLayersMigrateDatabaseChangeStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        var file = context.Files.FirstOrDefault(file => file.Name.EndsWith("/migrate-database.ps1"));
        file?.SetContent(file.Content.Replace("MyCompanyName.MyProjectName", "MyCompanyName.MyProjectName.Host"));
    }
}
