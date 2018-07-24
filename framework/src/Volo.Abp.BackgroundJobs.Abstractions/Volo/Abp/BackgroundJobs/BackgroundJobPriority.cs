namespace Volo.Abp.BackgroundJobs
{
    /// <summary>
    /// Priority of a background job.
    /// </summary>
    public enum BackgroundJobPriority : byte
    {
        /// <summary>
        /// Low.
        /// </summary>
        Low = 5,

        /// <summary>
        /// Below normal.
        /// </summary>
        BelowNormal = 10,

        /// <summary>
        /// Normal (default).
        /// </summary>
        Normal = 15,

        /// <summary>
        /// Above normal.
        /// </summary>
        AboveNormal = 20,

        /// <summary>
        /// High.
        /// </summary>
        High = 25
    }
}