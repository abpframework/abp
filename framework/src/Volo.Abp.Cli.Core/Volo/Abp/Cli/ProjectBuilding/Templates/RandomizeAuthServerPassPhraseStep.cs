using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates;

public class RandomizeAuthServerPassPhraseStep : ProjectBuildPipelineStep
{
    protected const string DefaultPassPhrase = "00000000-0000-0000-0000-000000000000";

    public override void Execute(ProjectBuildContext context)
    {
        var appSettings = context.Files
            .Where(x => !x.IsDirectory)
            .Where(x => x.Content.IndexOf(DefaultPassPhrase, StringComparison.InvariantCultureIgnoreCase) >= 0)
            .ToList();

        var randomPassPhrase = Guid.NewGuid().ToString("D");
        foreach (var appSetting in appSettings)
        {
            appSetting.NormalizeLineEndings();

            var appSettingLines = appSetting.GetLines();
            for (var i = 0; i < appSettingLines.Length; i++)
            {
                if (appSettingLines[i].Contains(DefaultPassPhrase))
                {
                    appSettingLines[i] = appSettingLines[i].Replace(DefaultPassPhrase, randomPassPhrase);
                }
            }

            appSetting.SetLines(appSettingLines);
        }
    }
}
