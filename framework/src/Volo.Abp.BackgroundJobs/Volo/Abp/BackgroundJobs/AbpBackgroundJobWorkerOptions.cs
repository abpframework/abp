namespace Volo.Abp.BackgroundJobs;

public class AbpBackgroundJobWorkerOptions
{
    /// <summary>
    /// Interval between polling jobs from <see cref="IBackgroundJobStore"/>.
    /// Default value: 5000 (5 seconds).
    /// </summary>
    public int JobPollPeriod { get; set; }

    /// <summary>
    /// Maximum count of jobs to fetch from data store in one loop.
    /// Default: 1000.
    /// </summary>
    public int MaxJobFetchCount { get; set; }

    /// <summary>
    /// Default duration (as seconds) for the first wait on a failure.
    /// Default value: 60 (1 minutes).
    /// </summary>
    public int DefaultFirstWaitDuration { get; set; }

    /// <summary>
    /// Default timeout value (as seconds) for a job before it's abandoned (<see cref="BackgroundJobInfo.IsAbandoned"/>).
    /// Default value: 172,800 (2 days).
    /// </summary>
    public int DefaultTimeout { get; set; }

    /// <summary>
    /// Default wait factor for execution failures.
    /// This amount is multiplated by last wait time to calculate next wait time.
    /// Default value: 2.0.
    /// </summary>
    public double DefaultWaitFactor { get; set; }

    public AbpBackgroundJobWorkerOptions()
    {
        MaxJobFetchCount = 1000;
        JobPollPeriod = 5000;
        DefaultFirstWaitDuration = 60;
        DefaultTimeout = 172800;
        DefaultWaitFactor = 2.0;
    }
}
