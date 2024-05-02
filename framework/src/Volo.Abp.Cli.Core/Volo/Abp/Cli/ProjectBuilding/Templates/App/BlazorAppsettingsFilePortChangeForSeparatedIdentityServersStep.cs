using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class BlazorAppsettingsFilePortChangeForSeparatedIdentityServersStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        var appsettingsFile = context.Files.FirstOrDefault(x =>
            !x.IsDirectory &&
            x.Name.EndsWith("aspnet-core/src/MyCompanyName.MyProjectName.Blazor/wwwroot/appsettings.json",
                StringComparison.InvariantCultureIgnoreCase)
        );

        appsettingsFile.NormalizeLineEndings();
        var lines = appsettingsFile.GetLines();

        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            var previousLine = lines[i-1];

            if (line.Contains("Authority") && line.Contains("localhost"))
            {
                line = line.Replace("44305", "44301");
            }
            else if (previousLine.Contains("AbpAccountPublic") && line.Contains("BaseUrl") && line.Contains("localhost"))
            {
                line = line.Replace("44305", "44301");
            }
            else if (previousLine.Contains("Default") && line.Contains("BaseUrl") && line.Contains("localhost"))
            {
                line = line.Replace("44305", "44300");
            }

            lines[i] = line;
        }

        appsettingsFile.SetLines(lines);
    }
}
