using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class ChangePublicAuthPortStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        var publicAppSettings = context.Files
            .FirstOrDefault(f => f.Name.Contains("MyCompanyName.MyProjectName.Web.Public") && f.Name.EndsWith("appsettings.json"));

        if (context.BuildArgs.UiFramework == UiFramework.BlazorServer)
        {
            publicAppSettings?.SetContent(publicAppSettings.Content.Replace("localhost:44303", "localhost:44313"));
        }
        else
        {
            publicAppSettings?.SetContent(publicAppSettings.Content.Replace("localhost:44303", "localhost:44305"));
        }
    }
}
