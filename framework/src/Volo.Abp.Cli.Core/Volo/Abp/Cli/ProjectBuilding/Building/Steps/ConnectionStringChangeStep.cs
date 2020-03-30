using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class ConnectionStringChangeStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            var newConnectionString = "\"Default\": \"" + context.BuildArgs.ConnectionString + "\"";

            var appSettingsJsonFiles = context.Files.Where(f =>
                f.Name.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase));

            foreach (var appSettingsJson in appSettingsJsonFiles)
            {
                try
                {
                    var jsonObject = JObject.Parse(appSettingsJson.Content);
                    var connectionStringContainer = (JContainer)jsonObject["ConnectionStrings"];
                    var firstConnectionString = connectionStringContainer.First;
                    var defaultConnectionString = firstConnectionString.ToString();

                    appSettingsJson.ReplaceText(defaultConnectionString, newConnectionString);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot change the connection string in " + appSettingsJson.Name + ". Error: " + ex.Message);
                }
            }
        }
    }
}
