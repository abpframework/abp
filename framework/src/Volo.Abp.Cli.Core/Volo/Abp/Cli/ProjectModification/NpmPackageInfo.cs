namespace Volo.Abp.Cli.ProjectModification;

public class NpmPackageInfo
{
    public string Name { get; set; }

    public NpmApplicationType ApplicationType { get; set; }

    public string MinVersion { get; set; }

    public string MaxVersion { get; set; }
}
