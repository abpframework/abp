namespace Volo.Abp.Cli.Build;

public class DotNetProjectBuildConfig
{
    public string BuildName { get; set; }

    public string SlFilePath { get; set; }

    public GitRepository GitRepository { get; set; }

    public bool ForceBuild { get; set; }
}
