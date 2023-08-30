using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates;

public class RandomizeAuthServerPassPhraseStep : ProjectBuildPipelineStep
{
    protected const string DefaultPassPhrase = "00000000-0000-0000-0000-000000000000";

    public readonly static string RandomPassPhrase = Guid.NewGuid().ToString("D");

    public override void Execute(ProjectBuildContext context)
    {
        var files = context.Files
            .Where(x => !x.IsDirectory)
            .Where(x => x.Content.IndexOf(DefaultPassPhrase, StringComparison.InvariantCultureIgnoreCase) >= 0)
            .ToList();

        var modules = new List<string>();
        foreach (var file in files)
        {
            file.NormalizeLineEndings();

            var lines = file.GetLines();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(DefaultPassPhrase))
                {
                    lines[i] = lines[i].Replace(DefaultPassPhrase, RandomPassPhrase);
                    if (file.Name.EndsWith("Module.cs", StringComparison.OrdinalIgnoreCase) &&
                        file.Content.Contains("openiddict.pfx", StringComparison.OrdinalIgnoreCase))
                    {
                        modules.Add(file.Name);
                    }
                }
            }

            file.SetLines(lines);
        }

        context.BuildArgs.ExtraProperties.TryAdd(nameof(RandomizeAuthServerPassPhraseStep), string.Empty);
        context.BuildArgs.ExtraProperties[nameof(RandomizeAuthServerPassPhraseStep)] += modules.JoinAsString(Environment.NewLine);
    }
}
