namespace Volo.Abp.BackgroundJobs;

[BackgroundJobName(JobNameConstant)]
public class DynamicBackgroundJobArgs
{
    public const string JobNameConstant = "Abp.DynamicJob";

    public string JobName { get; private set; }

    public string JsonData { get; private set; }

    // For serializers that require a parameterless constructor (e.g. System.Text.Json)
    private DynamicBackgroundJobArgs()
    {
        JobName = string.Empty;
        JsonData = string.Empty;
    }

    public DynamicBackgroundJobArgs(string jobName, string jsonData)
    {
        JobName = Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        JsonData = Check.NotNull(jsonData, nameof(jsonData));
    }
}
