using NuGet.Versioning;

namespace Volo.Abp.Cli.Version;

public class LatestVersionInfo
{
    public SemanticVersion Version { get; }

    public string Message { get; }

    public LatestVersionInfo(SemanticVersion version, string message = null)
    {
        Version = version;
        Message = message;
    }
}