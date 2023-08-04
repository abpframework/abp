using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Maui;

public class MauiChangePortStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        var appsettingsFile = context.Files.FirstOrDefault(x =>
            !x.IsDirectory &&
            x.Name.EndsWith("aspnet-core/src/MyCompanyName.MyProjectName.Maui/appsettings.json",
                StringComparison.InvariantCultureIgnoreCase)
        );

        if (appsettingsFile == null)
        {
            return;
        }

        var ports = GetPorts(context);
        
        appsettingsFile.NormalizeLineEndings();
        var lines = appsettingsFile.GetLines();

        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            var previousLine = lines[i-1];

            if (line.Contains("Authority") && line.Contains("localhost"))
            {
                line = line.Replace("44305", ports.AuthServerPort);
            }
            else if (previousLine.Contains("Default") && line.Contains("BaseUrl") && line.Contains("localhost"))
            {
                line = line.Replace("44305", ports.ApiHostPort);
            }

            lines[i] = line;
        }

        appsettingsFile.SetLines(lines);
    }
    
    private (string AuthServerPort, string ApiHostPort) GetPorts(ProjectBuildContext context)
    {
        var authServerPort = string.Empty;
        var apiHostPort = string.Empty;

        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.Angular:
            case UiFramework.Blazor:
            case UiFramework.MauiBlazor:
                authServerPort = "44305";
                apiHostPort = "44305";
                break;
            case UiFramework.BlazorServer:
                authServerPort = "44308";
                apiHostPort = "44308";
                break;
            case UiFramework.Mvc:
            case UiFramework.NotSpecified:
                authServerPort = "44303";
                apiHostPort = "44303";
                break;
        }

        if (context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server") ||
            context.BuildArgs.ExtraProperties.ContainsKey("separate-auth-server") ||
            context.BuildArgs.ExtraProperties.ContainsKey("tiered"))
        {
            authServerPort = "44301";
            apiHostPort = "44300";
        }

        return (authServerPort, apiHostPort);
    }
}