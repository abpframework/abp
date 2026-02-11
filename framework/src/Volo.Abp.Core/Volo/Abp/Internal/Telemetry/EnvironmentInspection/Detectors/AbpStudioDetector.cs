using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Internal.Telemetry.Constants.Enums;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Contracts;
using Volo.Abp.Internal.Telemetry.EnvironmentInspection.Core;

namespace Volo.Abp.Internal.Telemetry.EnvironmentInspection.Detectors;

internal sealed class AbpStudioDetector : SoftwareDetector
{
    public override string Name => "Abp Studio";
    private const string AbpStudioVersionExtensionName = "Volo.Abp.Studio.Extensions.StandardSolutionTemplates";

    public override Task<SoftwareInfo?> DetectAsync()
    {
        try
        {
            var uiTheme = GetAbpStudioUiTheme();
            var version = GetAbpStudioVersion();

            return Task.FromResult<SoftwareInfo?>(new SoftwareInfo(Name, version, uiTheme, SoftwareType.AbpStudio));
        }
        catch
        {
            return Task.FromResult<SoftwareInfo?>(null);
        }
    }

    private string? GetAbpStudioUiTheme()
    {
        var ideStateJsonPath = Path.Combine(
            TelemetryPaths.Studio,
            "ui",
            "ide-state.json"
        );
        if (!File.Exists(ideStateJsonPath))
        {
            return null;
        }
        using var fs = new FileStream(ideStateJsonPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var doc = JsonDocument.Parse(fs, new JsonDocumentOptions
        {
            AllowTrailingCommas = true
        });

        return doc.RootElement.TryGetProperty("theme", out var themeElement) ? themeElement.GetString() : null;
    }

    private string? GetAbpStudioVersion()
    {
        var extensionsFilePath = Path.Combine(TelemetryPaths.Studio, "extensions.json");

        if (!File.Exists(extensionsFilePath))
        {
            return null;
        }

        using var fs = new FileStream(extensionsFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var doc = JsonDocument.Parse(fs, new JsonDocumentOptions
        {
            AllowTrailingCommas = true
        });

        if (doc.RootElement.TryGetProperty("Extensions", out var extensionsElement) &&
            extensionsElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var extension in extensionsElement.EnumerateArray())
            {
                if (extension.TryGetProperty("name", out var nameProp) &&
                    nameProp.GetString() == AbpStudioVersionExtensionName &&
                    extension.TryGetProperty("version", out var versionProp))
                {
                    return versionProp.GetString();
                }
            }
        }

        return null;
    }
}