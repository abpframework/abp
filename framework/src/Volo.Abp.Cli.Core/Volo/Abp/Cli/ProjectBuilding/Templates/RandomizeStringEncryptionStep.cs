using System;
using System.Linq;
using System.Text;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates;

public class RandomizeStringEncryptionStep : ProjectBuildPipelineStep
{
    protected const string DefaultPassPhrase = "gsKnGZ041HLL4IM8";

    public override void Execute(ProjectBuildContext context)
    {
        var appSettings = context.Files
            .Where(x => !x.IsDirectory && x.Name.EndsWith("appSettings.json", StringComparison.InvariantCultureIgnoreCase))
            .Where(x => x.Content.IndexOf("StringEncryption", StringComparison.InvariantCultureIgnoreCase) >= 0)
            .ToList();

        var randomPassPhrase = GetRandomPassPhrase(context);
        foreach (var appSetting in appSettings)
        {
            appSetting.NormalizeLineEndings();

            var appSettingLines = appSetting.GetLines();
            for (var i = 0; i < appSettingLines.Length; i++)
            {
                if (appSettingLines[i].Contains("DefaultPassPhrase") && appSettingLines[i].Contains(DefaultPassPhrase))
                {
                    appSettingLines[i] = appSettingLines[i].Replace(DefaultPassPhrase, randomPassPhrase);
                }
            }

            appSetting.SetLines(appSettingLines);
        }
    }

    protected virtual string GetRandomPassPhrase(ProjectBuildContext context)
    {
        const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var builder = new StringBuilder();
        for (var i = 0; i < DefaultPassPhrase.Length; i++)
        {
            builder.Append(letters[RandomHelper.GetRandom(0, letters.Length)]);
        }
        return builder.ToString();
    }
}
