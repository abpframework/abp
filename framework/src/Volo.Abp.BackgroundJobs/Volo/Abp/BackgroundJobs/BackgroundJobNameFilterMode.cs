namespace Volo.Abp.BackgroundJobs;

public enum BackgroundJobNameFilterMode : byte
{
    /// <summary>
    /// No filter; all job names match.
    /// </summary>
    None = 0,

    /// <summary>
    /// Only the job names in the filter match.
    /// </summary>
    Include = 1,

    /// <summary>
    /// All job names except those in the filter match.
    /// </summary>
    Exclude = 2
}
