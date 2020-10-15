using System;
using System.Linq;
using System.Text;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates
{
    public class RandomizeStringEncryptionStep: ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            var appSettings = context.Files
                .Where(x => !x.IsDirectory && x.Name.EndsWith("appSettings.json", StringComparison.InvariantCultureIgnoreCase))
                .Where(x => x.Content.IndexOf("StringEncryption", StringComparison.InvariantCultureIgnoreCase) >= 0)
                .ToList();

            const string defaultPassPhrase = "gsKnGZ041HLL4IM8";
            var randomPassPhrase = GetRandomString(defaultPassPhrase.Length);
            foreach (var appSetting in appSettings)
            {
                appSetting.NormalizeLineEndings();

                var appSettingLines = appSetting.GetLines();
                for (var i = 0; i < appSettingLines.Length; i++)
                {
                    if (appSettingLines[i].Contains("DefaultPassPhrase") && appSettingLines[i].Contains(defaultPassPhrase))
                    {
                        appSettingLines[i] = appSettingLines[i].Replace(defaultPassPhrase, randomPassPhrase);
                    }
                }

                appSetting.SetLines(appSettingLines);
            }
        }

        private static string GetRandomString(int length)
        {
            const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append(letters[RandomHelper.GetRandom(0, letters.Length)]);
            }
            return builder.ToString();
        }
    }
}
