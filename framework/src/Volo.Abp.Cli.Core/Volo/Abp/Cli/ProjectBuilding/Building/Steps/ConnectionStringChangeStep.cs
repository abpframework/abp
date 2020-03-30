using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Volo.Abp.Text;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class ConnectionStringChangeStep : ProjectBuildPipelineStep
    {
        private const string DefaultConnectionStringKey = "Default";

        public override void Execute(ProjectBuildContext context)
        {
            var appSettingsJsonFiles = context.Files.Where(f =>
                f.Name.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (!appSettingsJsonFiles.Any())
            {
                return;
            }

            var newConnectionString = $"\"{DefaultConnectionStringKey}\": \"{context.BuildArgs.ConnectionString}\"";

            foreach (var appSettingsJson in appSettingsJsonFiles)
            {
                try
                {
                    var appSettingJsonContentWithoutBom = StringHelper.ConvertFromBytesWithoutBom(appSettingsJson.Bytes);
                    var jsonObject = JObject.Parse(appSettingJsonContentWithoutBom);

                    var connectionStringContainer = (JContainer)jsonObject?["ConnectionStrings"];
                    if (connectionStringContainer == null)
                    {
                        continue;
                    }

                    if (!connectionStringContainer.Any())
                    {
                        continue;
                    }

                    var connectionStrings = connectionStringContainer.ToList();

                    foreach (var connectionString in connectionStrings)
                    {
                        var property = ((JProperty)connectionString);
                        var connectionStringName = property.Name;

                        if (connectionStringName == DefaultConnectionStringKey)
                        {
                            var defaultConnectionString = property.ToString();
                            if (defaultConnectionString == null)
                            {
                                continue;
                            }

                            appSettingsJson.ReplaceText(defaultConnectionString, newConnectionString);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot change the connection string in " + appSettingsJson.Name + ". Error: " + ex.Message);
                }
            }
        }
    }
}
