using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App
{
    public class ReactEnvironmentFilePortChangeForSeparatedIdentityServersStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            var fileEntry = context.Files.FirstOrDefault(x =>
                !x.IsDirectory &&
                x.Name.EndsWith($"{MobileApp.ReactNative.GetFolderName()}/Environment.js",
                    StringComparison.InvariantCultureIgnoreCase)
            );

            if (fileEntry == null)
            {
                return;
            }

            fileEntry.NormalizeLineEndings();
            var lines = fileEntry.GetLines();

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (line.Contains("issuer") && line.Contains("localhost"))
                {
                    line = line.Replace("44305", "44301");
                }
                else if (line.Contains("apiUrl") && line.Contains("localhost"))
                {
                    line = line.Replace("44305", "44300");
                }

                lines[i] = line;
            }

            fileEntry.SetLines(lines);
        }
    }
}
