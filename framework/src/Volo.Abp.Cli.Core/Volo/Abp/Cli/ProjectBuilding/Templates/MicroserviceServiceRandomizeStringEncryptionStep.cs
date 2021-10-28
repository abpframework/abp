using System;
using System.IO;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates
{
    public class MicroserviceServiceRandomizeStringEncryptionStep : RandomizeStringEncryptionStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            var appSettings = context.Files
                .Where(x => !x.IsDirectory && x.Name.EndsWith("appSettings.json", StringComparison.InvariantCultureIgnoreCase))
                .Where(x => x.Content.IndexOf("StringEncryption", StringComparison.InvariantCultureIgnoreCase) >= 0)
                .ToList();

            const string defaultPassPhrase = "gsKnGZ041HLL4IM8";
            var randomPassPhrase = FindDefaultPassPhrase(context) ?? GetRandomString(defaultPassPhrase.Length);
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

        protected static string FindDefaultPassPhrase(ProjectBuildContext context)
        {
            var directoryInfo = new DirectoryInfo(context.BuildArgs.OutputFolder);
            do
            {
                var msSolution = Directory.GetFiles(directoryInfo.FullName, "*.sln", SearchOption.TopDirectoryOnly).FirstOrDefault();
                if (msSolution != null)
                {
                    var appSettings = Directory.GetFiles(Path.Combine(directoryInfo.FullName, "apps", "auth-server"),
                        "appsettings.json", SearchOption.AllDirectories).FirstOrDefault();
                    if (appSettings != null)
                    {
                        var file = File.ReadAllText(appSettings);
                        const string searchText = "DefaultPassPhrase\": \"";
                        var s = file.IndexOf(searchText, StringComparison.Ordinal) + searchText.Length;
                        var e = file.IndexOf("\"", s, StringComparison.Ordinal);
                        var defaultPassPhrase = file.Substring(s, e - s);
                        return defaultPassPhrase;
                    }
                }
                directoryInfo = directoryInfo.Parent;
            } while (directoryInfo?.Parent != null);

            return null;
        }
    }
}
