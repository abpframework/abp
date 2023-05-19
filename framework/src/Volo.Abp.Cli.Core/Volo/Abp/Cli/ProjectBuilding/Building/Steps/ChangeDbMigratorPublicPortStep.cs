using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class ChangeDbMigratorPublicPortStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        var dbMigratorAppSettings = context.Files
            .FirstOrDefault(f => f.Name.Contains("MyCompanyName.MyProjectName.DbMigrator") && f.Name.EndsWith("appsettings.json"));

        dbMigratorAppSettings?.SetContent(dbMigratorAppSettings.Content.Replace("localhost:44304", "localhost:44306"));
    }
}
