using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                    var appSettingJsonContentWithoutBom = GetStringWithoutBom(appSettingsJson.Bytes);
                    var jsonObject = JObject.Parse(appSettingJsonContentWithoutBom);
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

        private static string GetStringWithoutBom(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            
            var hasBom =  bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;

            if (hasBom)
            {
                return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            }
            else
            {
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}
