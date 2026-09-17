using System;
using System.Threading.Tasks;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Core;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Detectors;

internal class NodeJsDetector : SoftwareDetector
{
    public override string Name => "Node.js";

    public async override Task<SoftwareInfo?> DetectAsync()
    {
        try
        {
            var output = await ExecuteCommandAsync("node", "-v");

            if (output.IsNullOrWhiteSpace())
            {
                return null;
            }

            var version = output.Trim().TrimStart('v');

            return new SoftwareInfo(Name, version, uiTheme: null, SoftwareType.Others);
        }
        catch
        {
            return null;
        }
    }
}