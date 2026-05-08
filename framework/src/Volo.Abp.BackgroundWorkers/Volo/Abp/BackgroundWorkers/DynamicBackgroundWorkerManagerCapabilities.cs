namespace Volo.Abp.BackgroundWorkers;

public class DynamicBackgroundWorkerManagerCapabilities
{
    public bool SupportsDynamicRegistration { get; set; } = true;

    public bool SupportsCronExpression { get; set; } = true;
}
