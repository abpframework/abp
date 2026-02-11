using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates;

public class RemoveUnnecessaryPortsStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        RemoveUnnecessaryHttpApiHostPorts(context);
    }

    private static void RemoveUnnecessaryHttpApiHostPorts(ProjectBuildContext context)
    {
        var httpApiHostAppSettings = context.Files.FirstOrDefault(f => f.Name.EndsWith(".HttpApi.Host/appsettings.json"));

        if (httpApiHostAppSettings == null)
        {
            return;
        }

        var lines = httpApiHostAppSettings.GetLines();
        var newlines = new List<string>();
        var portsToRemove = new List<string>();
        var removeAngularUrl = false;

        var appSettingsJson = JObject.Parse(httpApiHostAppSettings.Content);
        var appJson = (JObject)appSettingsJson["App"];

        if (context.BuildArgs.UiFramework != UiFramework.Angular)
        {
            removeAngularUrl = true;
            portsToRemove.Add("http://localhost:4200");
        }
        
        if (context.BuildArgs.UiFramework != UiFramework.Blazor)
        {
            portsToRemove.Add("https://localhost:44307");
        }

        foreach (var line in lines)
        {
            var newLine = line;
            if(removeAngularUrl && (line.Contains("AngularUrl")|| line.Contains("ClientUrl")))
            {
                continue;
            }
            
            if(line.Contains("CorsOrigins") || line.Contains("RedirectAllowedUrls"))
            {
                var jsonValue = line.Contains("CorsOrigins") ? appJson["CorsOrigins"]?.ToString() : appJson["RedirectAllowedUrls"]?.ToString();
                if(!jsonValue.IsNullOrWhiteSpace())
                {
                    newLine = line.Replace(jsonValue, string.Join(",", jsonValue.Split(',').Where(x => !portsToRemove.Contains(x))));
                }
            }
            
            newlines.Add(newLine);
        }

        httpApiHostAppSettings.SetLines(newlines);
    }
}
