using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates
{
    public class TemplateRandomSslPortStep : ProjectBuildPipelineStep
    {
        private readonly int _minSslPort;

        private readonly int _maxSslPort;

        private readonly List<string> _buildInSslUrls;

        public TemplateRandomSslPortStep(List<string> buildInSslSslUrls, int minSslPort = 44300,
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
                }
            }
        }

        private string GetRandomPort(IReadOnlyCollection<string> excludePort = null)
        {
            var ports = Enumerable.Range(_minSslPort, _maxSslPort).Select(p => p.ToString());

            return RandomHelper.GetRandomOfList(excludePort != null
                ? ports.Except(excludePort).ToList()
                : ports.ToList());
        }

        private string GetUrlPort(string url)
        {
            var match = Regex.Match(url, @":(\d+)");
            return match.Success ? match.Groups[1].Value : "";
        }

        private string GetUrlWithoutPort(string url)
        {
            var match = Regex.Match(url, @"(^.+):");
            return match.Success ? match.Groups[1].Value : "";
        }
    }
}