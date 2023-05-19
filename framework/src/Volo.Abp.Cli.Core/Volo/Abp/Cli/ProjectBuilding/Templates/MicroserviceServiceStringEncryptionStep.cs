using System;
using System.IO;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates;

public class MicroserviceServiceStringEncryptionStep : RandomizeStringEncryptionStep
{
    protected override string GetRandomPassPhrase(ProjectBuildContext context)
    {
        return FindDefaultPassPhrase(context) ?? base.GetRandomPassPhrase(context);
    }

    protected virtual string FindDefaultPassPhrase(ProjectBuildContext context)
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
