using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Core;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Detectors;

internal sealed class OperatingSystemDetector : SoftwareDetector
{
    public override string Name => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" :
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "macOS" : "Linux";

    public async override Task<SoftwareInfo?> DetectAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new SoftwareInfo(Name, Environment.OSVersion.Version.ToString(), null, SoftwareType.OperatingSystem);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var version = await ExecuteCommandAsync("sw_vers", "-productVersion");
            return new SoftwareInfo(Name, version, GetMacUiTheme(), SoftwareType.OperatingSystem);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var version = await ExecuteCommandAsync("lsb_release", "-ds") ?? await ExecuteCommandAsync("uname", "-r");
            return new SoftwareInfo(Name, version, await GetLinuxUiTheme(), SoftwareType.OperatingSystem);
        }

        return null;
    }

    private async Task<string?> GetLinuxUiTheme()
    {
        var output = await ExecuteCommandAsync("gsettings", "get org.gnome.desktop.interface gtk-theme");

        if (!output.IsNullOrWhiteSpace() && output.ToLowerInvariant().Contains("dark"))
        {
            return "Dark";
        }

        return "Light";
    }


    private string? GetMacUiTheme()
    {
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "defaults",
                Arguments = "read -g AppleInterfaceStyle",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            return output == "Dark" ? "Dark" : "Light";
        }
        catch
        {
            return "Light";
        }
    }
}