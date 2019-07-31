using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App
{
    public class AppTemplateChangeConsoleTestClientPortSettingsStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            context
                .GetFile("/aspnet-core/test/MyCompanyName.MyProjectName.HttpApi.Client.ConsoleTestApp/appsettings.json")
                .ReplaceText("44395", "44361")
                .ReplaceText("44348", "44361");
        }
    }
}
