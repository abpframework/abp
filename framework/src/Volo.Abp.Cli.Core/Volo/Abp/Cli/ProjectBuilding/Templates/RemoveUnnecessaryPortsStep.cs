using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates
{
    public class RemoveUnnecessaryPortsStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
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
                        appJson["CorsOrigins"].ToString().Split(",").Where(u=> !portsToRemoveFromCors.Any(u.EndsWith))
                    );
            }

            httpApiHostAppSettings.SetContent(JsonConvert.SerializeObject(appSettingsJson, Formatting.Indented));
        }
    }
}
