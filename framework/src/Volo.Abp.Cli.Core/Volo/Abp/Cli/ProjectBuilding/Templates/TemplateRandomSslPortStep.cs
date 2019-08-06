using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;

namespace Volo.Abp.Cli.ProjectBuilding.Templates
{
    public class TemplateRandomSslPortStep : ProjectBuildPipelineStep
    {
        private readonly int _minSslPort;

        private readonly int _maxSslPort;

        private readonly List<string> _buildInSslUrls;

        public TemplateRandomSslPortStep(
            List<string> buildInSslSslUrls,
            int minSslPort = 44300,
            int maxSslPort = 44399)
        {
            _buildInSslUrls = buildInSslSslUrls;

            _minSslPort = minSslPort;
            _maxSslPort = maxSslPort;
        }

        public override void Execute(ProjectBuildContext context)
        {
            var launchSettings = context.Files.Where(x =>
                    !x.IsDirectory && x.Name.EndsWith("launchSettings.json", StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            var appSettings = context.Files.Where(x =>
                    !x.IsDirectory && x.Name.EndsWith("appSettings.json", StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            var angularEnvironments = context.Files.Where(x =>
                    !x.IsDirectory &&
                    (x.Name.EndsWith("environments/environment.ts", StringComparison.InvariantCultureIgnoreCase) ||
                    x.Name.EndsWith("environments/environment.hmr.ts", StringComparison.InvariantCultureIgnoreCase) ||
                    x.Name.EndsWith("environments/environment.prod.ts", StringComparison.InvariantCultureIgnoreCase))
                )
                .ToList();

            if (AppTemplateBase.IsAppTemplate(context.Template.Name))
            {
                // no tiered
                if (launchSettings.Count == 1 &&
                    launchSettings.First().Name
                        .Equals("/aspnet-core/src/MyCompanyName.MyProjectName.Web/Properties/launchSettings.json", StringComparison.InvariantCultureIgnoreCase))
                {
                    var dbMigrator = appSettings.FirstOrDefault(x =>
                        x.Name.Equals("/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator/appsettings.json", StringComparison.InvariantCultureIgnoreCase));
                    dbMigrator?.SetContent(dbMigrator.Content.Replace("https://localhost:44302", "https://localhost:44303"));

                    var web = appSettings.FirstOrDefault(x =>
                        x.Name.Equals("/aspnet-core/src/MyCompanyName.MyProjectName.Web/appsettings.json", StringComparison.InvariantCultureIgnoreCase));
                    web?.SetContent(web.Content.Replace("https://localhost:44301", "https://localhost:44303"));

                    var consoleTestApp = appSettings.FirstOrDefault(x =>
                        x.Name.Equals("/aspnet-core/test/MyCompanyName.MyProjectName.HttpApi.Client.ConsoleTestApp/appsettings.json", StringComparison.InvariantCultureIgnoreCase));
                    consoleTestApp?.SetContent(consoleTestApp.Content.Replace("https://localhost:44300", "https://localhost:44303"));
                    consoleTestApp?.SetContent(consoleTestApp.Content.Replace("https://localhost:44301", "https://localhost:44303"));
                }
            }

            var excludePorts = new List<string>();
            excludePorts.AddRange(_buildInSslUrls.Select(GetUrlPort).ToList());

            foreach (var buildInUrl in _buildInSslUrls)
            {
                var newPort = GetRandomPort(excludePorts);
                excludePorts.Add(newPort);

                var buildInUrlWithoutPort = GetUrlWithoutPort(buildInUrl);
                var buildInUrlPort = GetUrlPort(buildInUrl);

                foreach (var launchSetting in launchSettings)
                {
                    launchSetting.NormalizeLineEndings();

                    var launchSettingLines = launchSetting.GetLines();
                    for (var i = 0; i < launchSettingLines.Length; i++)
                    {
                        if (launchSettingLines[i].Contains($"{buildInUrl}"))
                        {
                            launchSettingLines[i] = launchSettingLines[i].Replace($"{buildInUrl}", $"{buildInUrlWithoutPort}:{newPort}");
                        }

                        if (launchSettingLines[i].Contains($"\"sslPort\": {buildInUrlPort}"))
                        {
                            launchSettingLines[i] = launchSettingLines[i]
                                .Replace($"\"sslPort\": {buildInUrlPort}", $"\"sslPort\": {newPort}");
                        }
                    }
                    launchSetting.SetLines(launchSettingLines);

                    foreach (var appSetting in appSettings)
                    {
                        appSetting.NormalizeLineEndings();

                        var appSettingLines = appSetting.GetLines();
                        for (var i = 0; i < appSettingLines.Length; i++)
                        {
                            if (appSettingLines[i].Contains(buildInUrl))
                            {
                                appSettingLines[i] = appSettingLines[i].Replace(buildInUrl, $"{buildInUrlWithoutPort}:{newPort}");
                            }
                        }
                        appSetting.SetLines(appSettingLines);
                    }

                    foreach (var environment in angularEnvironments)
                    {
                        environment.NormalizeLineEndings();

                        var environmentLines = environment.GetLines();
                        for (var i = 0; i < environmentLines.Length; i++)
                        {
                            if (environmentLines[i].Contains(buildInUrl))
                            {
                                environmentLines[i] = environmentLines[i].Replace(buildInUrl, $"{buildInUrlWithoutPort}:{newPort}");
                            }
                        }
                        environment.SetLines(environmentLines);
                    }

                }
            }
        }

        private string GetRandomPort(IReadOnlyCollection<string> excludePort = null)
        {
            var ports = Enumerable.Range(_minSslPort, _maxSslPort - _minSslPort + 1).Select(p => p.ToString());

            return RandomHelper.GetRandomOfList(excludePort != null
                ? ports.Except(excludePort).ToList()
                : ports.ToList());
        }

        private static string GetUrlPort(string url)
        {
            var match = Regex.Match(url, @":(\d+)");
            return match.Success ? match.Groups[1].Value : "";
        }

        private static string GetUrlWithoutPort(string url)
        {
            var match = Regex.Match(url, @"(^.+):");
            return match.Success ? match.Groups[1].Value : "";
        }
    }
}