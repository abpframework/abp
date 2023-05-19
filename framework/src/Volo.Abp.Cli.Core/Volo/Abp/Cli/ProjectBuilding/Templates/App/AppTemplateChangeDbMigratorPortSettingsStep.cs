using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class AppTemplateChangeDbMigratorPortSettingsStep : ProjectBuildPipelineStep
{
    public string AuthServerPort { get; }

    /// <param name="authServerPort"></param>
    public AppTemplateChangeDbMigratorPortSettingsStep(
        string authServerPort)
    {
        AuthServerPort = authServerPort;
    }

    public override void Execute(ProjectBuildContext context)
    {
        context
            .GetFile("/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator/appsettings.json")
            .ReplaceText("44305", AuthServerPort);
    }
}
