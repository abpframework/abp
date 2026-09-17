using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Core;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Detectors;

internal sealed class VisualStudioDetector : SoftwareDetector
{
    public override string Name => "Visual Studio";

    public override Task<SoftwareInfo?> DetectAsync()
    {
        var version = GetVisualStudioVersionViaVsWhere();
        var theme = GetVisualStudioTheme();

        if (version == null)
        {
            return Task.FromResult<SoftwareInfo?>(null);
        }

        return Task.FromResult<SoftwareInfo?>(new SoftwareInfo(
            name: Name,
            version: version,
            uiTheme: theme,
            softwareType: SoftwareType.Ide));
    }

    private string? GetVisualStudioVersionViaVsWhere()
    {
        var vswherePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "Microsoft Visual Studio",
            "Installer",
            "vswhere.exe");

        if (!File.Exists(vswherePath))
        {
            return null;
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = vswherePath,
                Arguments = "-latest -property catalog_productDisplayVersion",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        var output = process.StandardOutput.ReadToEnd().Trim();
        process.WaitForExit();

        return string.IsNullOrWhiteSpace(output) ? null : output;
    }

    private string? GetVisualStudioTheme()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        var vsSettingsDir = Path.Combine(localAppData, "Microsoft", "VisualStudio");

        if (!Directory.Exists(vsSettingsDir))
        {
            return null;
        }

        var settingsPath = Directory.GetFiles(vsSettingsDir, "CurrentSettings*.vssettings", SearchOption.AllDirectories)
            .OrderByDescending(File.GetLastWriteTime)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(settingsPath))
        {
            return null;
        }

        try
        {
            var doc = XDocument.Load(settingsPath);

            var themeId = doc.Descendants("Theme")
                .FirstOrDefault()?.Attribute("Id")?.Value;

            return themeId?.ToUpperInvariant() switch
            {
                "{1DED0138-47CE-435E-84EF-9EC1F439B749}" => "Dark",
                "{DE3DBBCD-F642-433C-8353-8F1DF4370ABA}" => "Light",
                "{2DED0138-47CE-435E-84EF-9EC1F439B749}" => "Blue",
                _ => "Unknown"
            };
        }
        catch
        {
            return null;
        }
    }
}