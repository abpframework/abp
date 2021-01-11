using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Templates
{
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
            var appJson = (JObject) appSettingsJson["App"];

            if (context.BuildArgs.UiFramework != UiFramework.Angular)
            {
                appJson.Property("ClientUrl")?.Remove();
                portsToRemoveFromCors.Add("4200");
            }

            if (context.BuildArgs.UiFramework != UiFramework.Blazor)
            {
                portsToRemoveFromCors.Add("44307");
            }

            if (appJson["CorsOrigins"] != null)
            {
                appJson["CorsOrigins"] = string.Join(",",
                    appJson["CorsOrigins"].ToString().Split(",").Where(u => !portsToRemoveFromCors.Any(u.EndsWith))
                );
            }

            httpApiHostAppSettings.SetContent(JsonConvert.SerializeObject(appSettingsJson, Formatting.Indented));
        }

        private static void RemoveUnnecessaryDbMigratorClients(ProjectBuildContext context)
        {
            var dbMigratorAppSettings = context.Files
                .FirstOrDefault(f =>
                    f.Name.Contains("MyCompanyName.MyProjectName.DbMigrator") && f.Name.EndsWith("appsettings.json"));

            var appSettingsJsonObject = JObject.Parse(dbMigratorAppSettings.Content);
            var identityServerJsonObject = (JObject) appSettingsJsonObject["IdentityServer"];
            var clientsJsonObject = (JObject) identityServerJsonObject["Clients"];

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
}
