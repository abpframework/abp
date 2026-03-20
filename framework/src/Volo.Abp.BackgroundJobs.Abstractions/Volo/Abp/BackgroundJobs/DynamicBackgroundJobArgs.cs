namespace Volo.Abp.BackgroundJobs;

[BackgroundJobName(JobNameConstant)]
public class DynamicBackgroundJobArgs
{
    public const string JobNameConstant = "Abp.DynamicJob";

    public string JobName { get; }

    public string JsonData { get; }

    public DynamicBackgroundJobArgs(string jobName, string jsonData)
    {
        JobName = Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        JsonData = Check.NotNull(jsonData, nameof(jsonData));
    }
}
