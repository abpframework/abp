using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Mvc
{
    public class MyTemplateChangeConsoleTestClientPortSettingsStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            context
                .GetFile("/test/MyCompanyName.MyProjectName.HttpApi.Client.ConsoleTestApp/appsettings.json")
                .ReplaceText("44395", "44361")
                .ReplaceText("44348", "44361");
        }
    }
}
