using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Core;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Detectors;

internal sealed class ChromeDetector : SoftwareDetector
{
    public override string Name => "Chrome";

    public async override Task<SoftwareInfo?> DetectAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var chromePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google", "Chrome", "Application", "chrome.exe");
            if (File.Exists(chromePath))
            {
                return new SoftwareInfo(Name, GetFileVersion(chromePath), null, SoftwareType.Browser);
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var chromePath = "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome";
            if (File.Exists(chromePath))
            {
                var version = await ExecuteCommandAsync(chromePath, "--version");
                return new SoftwareInfo(Name, version, null, SoftwareType.Browser);
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var chromePath = "/usr/bin/google-chrome";
            if (File.Exists(chromePath))
            {
                var version = await ExecuteCommandAsync(chromePath, "--version");
                return new SoftwareInfo(Name, version, null, SoftwareType.Browser);
            }
        }

        return null;
    }

  
}