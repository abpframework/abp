using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates;

public class RemoveUnnecessaryPortsStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        RemoveUnnecessaryDbMigratorClients(context);
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

    private static void RemoveUnnecessaryDbMigratorClients(ProjectBuildContext context)
    {
        var dbMigratorAppSettings = context.Files
            .FirstOrDefault(f =>
                f.Name.Contains("MyCompanyName.MyProjectName.DbMigrator") && f.Name.EndsWith("appsettings.json"));

        var appSettingsJsonObject = JObject.Parse(dbMigratorAppSettings.Content);
        var authServerJsonObject = (JObject)appSettingsJsonObject?["IdentityServer"] ?? (JObject)appSettingsJsonObject["OpenIddict"];
        var clientsJsonObject = (JObject)authServerJsonObject?["Clients"] ?? (JObject)authServerJsonObject?["Applications"];

        if (clientsJsonObject == null)
        {
            return;
        }

        if (context.BuildArgs.UiFramework != UiFramework.Blazor)
        {
            clientsJsonObject.Remove("MyProjectName_Blazor");
        }
        if (!context.BuildArgs.PublicWebSite)
        {
            clientsJsonObject.Remove("MyProjectName_Web_Public");
        }

        dbMigratorAppSettings.SetContent(appSettingsJsonObject.ToString(Formatting.Indented));
    }
}
