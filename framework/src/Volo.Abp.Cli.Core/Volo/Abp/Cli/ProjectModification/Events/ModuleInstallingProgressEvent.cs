namespace Volo.Abp.Cli.ProjectModification.Events;

public class ModuleInstallingProgressEvent
{
    public int CurrentStep { get; set; }

    public string Message { get; set; }
}
