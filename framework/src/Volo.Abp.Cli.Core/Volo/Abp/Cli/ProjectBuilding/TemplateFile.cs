namespace Volo.Abp.Cli.ProjectBuilding;

public class TemplateFile
{
    public string Version { get; }

    public string LatestVersion { get; }

    public string RepositoryNugetVersion { get; }

    public byte[] FileBytes { get; }

    public TemplateFile(byte[] fileBytes, string version, string latestVersion, string repositoryNugetVersion)
    {
        FileBytes = fileBytes;
        Version = version;
        LatestVersion = latestVersion;
        RepositoryNugetVersion = repositoryNugetVersion;
    }
}
