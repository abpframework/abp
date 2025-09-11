using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Core;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Detectors;

internal sealed class VisualStudioCodeDetector : SoftwareDetector
{
    public override string Name => "Visual Studio Code";

    public async override Task<SoftwareInfo?> DetectAsync()
    {
        string? installDir = null;
        string? settingsPath = null;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var progFiles    = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var candidates = new[]
            {
                Path.Combine(localAppData, "Programs", "Microsoft VS Code"),
                Path.Combine(progFiles,    "Microsoft VS Code")
            };
            installDir = candidates.FirstOrDefault(Directory.Exists);

            settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Code", "User", "globalStorage" ,"storage.json"
            );
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var app = "/Applications/Visual Studio Code.app";
            if (Directory.Exists(app))
            {
                installDir = app;
            }

            settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "Library", "Application Support", "Code", "User", "globalStorage",  "storage.json"
            );
        }
        else
        {
            var candidate = "/usr/share/code";
            if (Directory.Exists(candidate))
            {
                installDir = candidate;
            }

            settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                ".config", "Code", "User", "globalStorage",  "storage.json"
            );
        }

        if (installDir == null)
        {
            return null;
        }


        Version? version = null;
        var productJson = RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            ? Path.Combine(installDir, "Contents", "Resources", "app", "product.json")
            : Path.Combine(installDir, "resources", "app", "product.json");

        if (File.Exists(productJson))
        {
            try
            {
                using var jsonDoc = JsonDocument.Parse(File.ReadAllText(productJson));
                var root = jsonDoc.RootElement;
                if (root.TryGetProperty("version", out var versionProp))
                {
                    var versionStr = versionProp.GetString();
                    if (Version.TryParse(versionStr, out var v))
                    {
                        version = v;
                    }
                }
            }
            catch
            {
             
            }
        }

        if (version == null)
        {
            return null;
        }

        var theme = "Unknown";
        
        if (File.Exists(settingsPath))
        {
            try
            {
                using var json = JsonDocument.Parse( File.ReadAllText(settingsPath));
                var root = json.RootElement;
                if (root.TryGetProperty("theme", out var themeProp))
                {
                    var themeName = themeProp.GetString() ?? "";
                    if (themeName.IndexOf("dark", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        theme = "Dark";
                    }
                    else if (themeName.IndexOf("light", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        theme = "Light";
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        return new SoftwareInfo(Name, version?.ToString(), theme, SoftwareType.Ide);
        
    }
}