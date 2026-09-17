using Volo.Abp.Internal.Telemetry.Constants.Enums;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;

internal class SoftwareInfo(string name, string? version, string? uiTheme, SoftwareType softwareType)
{
    public string Name { get; set; } = name;
    public string? Version { get; set; } = version;
    public string? UiTheme { get; set; } = uiTheme;
    public SoftwareType SoftwareType { get; set; } = softwareType;
}