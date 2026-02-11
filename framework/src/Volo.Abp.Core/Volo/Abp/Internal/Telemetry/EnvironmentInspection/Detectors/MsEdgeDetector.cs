using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Core;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Detectors;

internal sealed class MsEdgeDetector : SoftwareDetector
{
    public override string Name => "MsEdge";
    public async override Task<SoftwareInfo?> DetectAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var firefoxPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "Microsoft", "Edge", "Application", "msedge.exe");
            
            if (File.Exists(firefoxPath))
            {
                return new SoftwareInfo(Name, GetFileVersion(firefoxPath), null, SoftwareType.Browser);
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var edgePath = "/Applications/Microsoft Edge.app/Contents/MacOS/Microsoft Edge";
            if (File.Exists(edgePath))
            {
                var version = await ExecuteCommandAsync(edgePath, "--version");
                return new SoftwareInfo(Name, version, null, SoftwareType.Browser);
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var edgePath = "/usr/bin/microsoft-edge";
            if (File.Exists(edgePath))
            {
                var version = await ExecuteCommandAsync(edgePath, "--version");
                return new SoftwareInfo(Name, version, null, SoftwareType.Browser);
            }
        }

        return null;
    }
}