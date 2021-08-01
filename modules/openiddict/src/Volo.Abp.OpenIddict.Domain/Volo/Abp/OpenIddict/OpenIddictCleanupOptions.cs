namespace Volo.Abp.BackgroundWorkers
{
    public class OpenIddictCleanupOptions
    {
        /// <summary>
        /// Default: 3,600,000 ms.
        /// </summary>
        public int CleanupPeriod { get; set; } = 3_600_000;

        /// <summary>
        /// Default value: 100.
        /// </summary>
        public int CleanupBatchSize { get; set; } = 100;

        /// <summary>
        /// The number of loop if there are
        /// more than <see cref="CleanupBatchSize"/> tokens in the database.
        /// So, if <see cref="CleanupLoopCount"/> is 10 and <see cref="CleanupBatchSize"/> is 100,
        /// then the cleanup worker will clean 1,000 items in one <see cref="CleanupPeriod"/> at max.
        ///
        /// Default value: 10.
        /// </summary>
        public int CleanupLoopCount { get; set; } = 10;

        /// <summary>
        /// Default value: true.
        /// If <see cref="AbpBackgroundWorkerOptions.IsEnabled"/> is false,
        /// this property is ignored and the authorization of cleanup worker doesn't work for this application instance.
        /// </summary>
        public bool IsCleanupAuthorizationEnabled { get; set; } = true;

        /// <summary>
        /// Default value: true.
        /// If <see cref="AbpBackgroundWorkerOptions.IsEnabled"/> is false,
        /// this property is ignored and the token of cleanup worker doesn't work for this application instance.
        /// </summary>
        public bool IsCleanupTokenEnabled { get; set; } = true;
    }
}