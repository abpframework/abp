using System.Collections.Generic;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

//TODO: Remove this step and move it to the ChangeThemeStep.cs?
public class ChangeThemeStyleStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        if (!context.BuildArgs.Theme.HasValue || context.BuildArgs.Theme != Theme.LeptonX)
        {
            return;
        }

        switch (context.BuildArgs.ThemeStyle)
        {
            case ThemeStyle.Light:
                ChangeThemeStyle(context, themeStyleName: "Light");
                break;
            case ThemeStyle.Dark:
                ChangeThemeStyle(context, themeStyleName: "Dark");
                break;
        }
    }

    private void ChangeThemeStyle(ProjectBuildContext context, string themeStyleName)
    {
        var defaultThemeStyleName = "LeptonXStyleNames.Dim";
        var newThemeStyleName = $"LeptonXStyleNames.{themeStyleName}";

        var filePaths = new List<string> 
        {
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyProjectNameWebModule.cs",
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyProjectNameHttpApiHostModule.cs",
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "/aspnet-core/src/MyCompanyName.MyProjectName.AuthServer/MyProjectNameAuthServerModule.cs"
        };

        foreach(var filePath in filePaths)
        {
            ReplaceThemeStyleName(context, filePath, defaultThemeStyleName, newThemeStyleName);
        }
    }

    protected void ReplaceThemeStyleName(ProjectBuildContext context, string filePath, string oldThemeStyleName, string newThemeStyleName)
    {
        var file = context.FindFile(filePath);
        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains(oldThemeStyleName))
            {
                lines[i] = lines[i].Replace(oldThemeStyleName, newThemeStyleName);
            }
        }

        file.SetLines(lines);
    }
}