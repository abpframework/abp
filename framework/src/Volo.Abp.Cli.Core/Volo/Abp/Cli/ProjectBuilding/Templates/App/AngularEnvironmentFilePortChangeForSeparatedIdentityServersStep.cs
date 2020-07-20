using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App
{
    public class AngularEnvironmentFilePortChangeForSeparatedIdentityServersStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            var fileEntries = context.Files.Where(x =>
                    !x.IsDirectory &&
                    (x.Name.EndsWith("angular/src/environments/environment.ts", StringComparison.InvariantCultureIgnoreCase) ||
                     x.Name.EndsWith("angular/src/environments/environment.hmr.ts", StringComparison.InvariantCultureIgnoreCase) ||
                     x.Name.EndsWith("angular/src/environments/environment.prod.ts", StringComparison.InvariantCultureIgnoreCase))
                )
                .ToList();

            foreach (var fileEntry in fileEntries)
            {
                fileEntry.NormalizeLineEndings();
                var lines = fileEntry.GetLines();

                for (var i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];

                    if (line.Contains("issuer") && line.Contains("localhost"))
                    {
                        line = line.Replace("44305", "44301");
                    }
                    else if (line.Contains("url") && line.Contains("localhost"))
                    {
                        line = line.Replace("44305", "44300");
                    }

                    lines[i] = line;
                }

                fileEntry.SetLines(lines);
            }
        }
    }
}
