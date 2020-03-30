using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class ConnectionStringChangeStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            var newConnectionString = context.BuildArgs.ConnectionString;

            var appSettingsJsonFiles = context.Files.Where(f =>
                f.Name.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase));

            foreach (var appSettingsJson in appSettingsJsonFiles)
            {
                var appSettingsObject = JsonConvert.DeserializeObject<AppSettingsConnectionStringModel>(appSettingsJson.Content);
                var oldConnectionString = appSettingsObject.ConnectionStrings.Default;
                appSettingsJson.ReplaceText(oldConnectionString, newConnectionString);
            }
        }

        public class ConnectionStringModel
        {
            public string Default { get; set; }
        }

        public class AppSettingsConnectionStringModel
        {
            public ConnectionStringModel ConnectionStrings { get; set; }

            public string GetDefaultConnectionString()
            {
                return ConnectionStrings?.Default;
            }
        }
    }
}
