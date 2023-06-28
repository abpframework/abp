using System;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates;

public class RandomizeAuthServerPassPhraseStep : ProjectBuildPipelineStep
{
    protected const string DefaultPassPhrase = "00000000-0000-0000-0000-000000000000";

    public override void Execute(ProjectBuildContext context)
    {
        var files = context.Files
            .Where(x => !x.IsDirectory)
            .Where(x => x.Content.IndexOf(DefaultPassPhrase, StringComparison.InvariantCultureIgnoreCase) >= 0)
            .ToList();

        var randomPassPhrase = Guid.NewGuid().ToString("D");
        foreach (var file in files)
        {
            file.NormalizeLineEndings();

            var lines = file.GetLines();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(DefaultPassPhrase))
                {
                    lines[i] = lines[i].Replace(DefaultPassPhrase, randomPassPhrase);
                }
            }

            file.SetLines(lines);
        }
    }
}
