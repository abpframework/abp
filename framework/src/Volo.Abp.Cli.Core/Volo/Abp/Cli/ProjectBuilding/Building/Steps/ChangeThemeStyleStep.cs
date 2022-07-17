using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class ChangeThemeStyleStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        if (context.BuildArgs.Theme != Theme.LeptonX)
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
            "/MyCompanyName.MyProjectName.Web/MyProjectNameWebModule.cs",
            "/MyCompanyName.MyProjectName.Web.Host/MyProjectNameWebModule.cs",
            "/MyCompanyName.MyProjectName.HttpApi.Host/MyProjectNameHttpApiHostModule.cs",
            "/MyCompanyName.MyProjectName.HttpApi.HostWithIds/MyProjectNameHttpApiHostModule.cs",
            "/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameBlazorModule.cs",
            "/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "/MyCompanyName.MyProjectName.Host/MyProjectNameModule.cs",
            "/MyCompanyName.MyProjectName.Host.Mongo/MyProjectNameModule.cs",
            "/MyCompanyName.MyProjectName.Mvc/MyProjectNameModule.cs",
            "/MyCompanyName.MyProjectName.Mvc.Mongo/MyProjectNameModule.cs",
            "/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "/MyCompanyName.MyProjectName.AuthServer/MyProjectNameAuthServerModule.cs",
        };

        foreach(var filePath in filePaths)
        {
            ChangeThemeStyleName(context, filePath, defaultThemeStyleName, newThemeStyleName);
        }
    }

    protected void ChangeThemeStyleName(ProjectBuildContext context, string filePath, string oldThemeStyleName, string newThemeStyleName)
    {
        var file = context.Files.FirstOrDefault(x => x.Name.Contains(filePath));
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