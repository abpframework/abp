namespace Volo.Abp.BackgroundJobs
{
    public static class BackgroundJobRecordConsts
    {
        public static int MaxJobNameLength { get; set; } = 128;

        public static int MaxJobArgsLength { get; set; } = 1024 * 1024;
    }
}
