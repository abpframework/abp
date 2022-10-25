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

        var portsToRemoveFromCors = new List<string>();

        var appSettingsJson = JObject.Parse(httpApiHostAppSettings.Content);
        var appJson = (JObject)appSettingsJson["App"];

        if (context.BuildArgs.UiFramework != UiFramework.Angular)
        {
            var clientUrl = appJson.Property("ClientUrl")?.ToString();
            portsToRemoveFromCors.Add("http://localhost:4200");
            
            if (!clientUrl.IsNullOrWhiteSpace())
            {
                httpApiHostAppSettings.SetContent(httpApiHostAppSettings.Content.Replace(clientUrl, string.Empty));
            }
        }
        
        if (context.BuildArgs.UiFramework != UiFramework.Blazor)
        {
            portsToRemoveFromCors.Add("https://localhost:44307");
        }

        
        if (appJson["CorsOrigins"] != null)
        {
            var corsOrigins = appJson["CorsOrigins"].ToString();
            var newCorsOrigins = string.Join(",", corsOrigins.Split(',').Where(x => !portsToRemoveFromCors.Contains(x)));
            
            httpApiHostAppSettings.SetContent(httpApiHostAppSettings.Content.Replace(corsOrigins, newCorsOrigins));
        }
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
