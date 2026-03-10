namespace Volo.Abp.BackgroundJobs;

[BackgroundJobName(JobNameConstant)]
public class AnonymousJobArgs
{
    public const string JobNameConstant = "Abp.AnonymousJob";

    public string JobName { get; }

    public string JsonData { get; }

    public AnonymousJobArgs(string jobName, string jsonData)
    {
        JobName = Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        JsonData = Check.NotNull(jsonData, nameof(jsonData));
    }
}
