namespace Volo.Abp.BackgroundJobs;

public static class BackgroundJobRecordConsts
{
    /// <summary>
    /// Default value: 128
    /// </summary>
    public static int MaxJobNameLength { get; set; } = 128;

    /// <summary>
    /// Default value: 1024 * 1024
    /// </summary>
    public static int MaxJobArgsLength { get; set; } = 1024 * 1024;
}
