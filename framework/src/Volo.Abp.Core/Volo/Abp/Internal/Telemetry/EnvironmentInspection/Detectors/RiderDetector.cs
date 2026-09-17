using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Core;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Detectors;

internal sealed class RiderDetector : SoftwareDetector
{
    public override string Name => "Rider";

    public override Task<SoftwareInfo?> DetectAsync()
    {
        try
        {
            string baseConfigDir;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                baseConfigDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "JetBrains");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                baseConfigDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    "Library", "Application Support", "JetBrains");
            }
            else
            {
                baseConfigDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    ".config", "JetBrains");
            }

            if (!Directory.Exists(baseConfigDir))
            {
                return Task.FromResult<SoftwareInfo?>(null);
            }

            var riderDirs = Directory
                .GetDirectories(baseConfigDir, "Rider*")
                .Select(dir =>
                {
                    var name = Path.GetFileName(dir);
                    var verStr = name.Substring("Rider".Length);
                    return Version.TryParse(verStr, out var v)
                        ? (Path: dir, Version: v)
                        : (Path: null, Version: null);
                })
                .Where(x => x.Path != null)
                .ToList();

            if (!riderDirs.Any())
            {
                return Task.FromResult<SoftwareInfo?>(null);
            }

            var latest = riderDirs
                .OrderByDescending(x => x.Version)
                .First();

            var theme = string.Empty;
            var colorsFile = Path.Combine(latest.Path!, "options", "colors.scheme.xml");
            if (File.Exists(colorsFile))
            {
                try
                {
                    var doc = XDocument.Load(colorsFile);
                    var schemeEl = doc
                        .Descendants("global_color_scheme")
                        .FirstOrDefault();
                    var schemeName = schemeEl?.Attribute("name")?.Value;
                    if (!schemeName.IsNullOrEmpty())
                    {
                        theme = schemeName.IndexOf("dark", StringComparison.OrdinalIgnoreCase) >= 0
                            ? "Dark"
                            : "Light";
                    }
                }
                catch
                {
                    //ignored
                }
            }

            return Task.FromResult<SoftwareInfo?>(new SoftwareInfo(Name, latest.Version?.ToString(), theme,
                SoftwareType.Ide));
        }
        catch (Exception e)
        {
            return Task.FromResult<SoftwareInfo?>(null);
        }
    }
}