using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs
{
    /// <summary>
    /// Represents a background job info that is used to persist jobs.
    /// </summary>
    public class BackgroundJobInfo
    {
        /// <summary>
        /// Maximum length of <see cref="JobName"/>.
        /// Value: 512.
        /// </summary>
        public const int MaxJobTypeLength = 512;

        /// <summary>
        /// Maximum length of <see cref="JobArgs"/>.
        /// Value: 1 MB (1,048,576 bytes).
        /// </summary>
        public const int MaxJobArgsLength = 1024 * 1024;

        /// <summary>
        /// Default duration (as seconds) for the first wait on a failure.
        /// Default value: 60 (1 minutes).
        /// </summary>
        public static int DefaultFirstWaitDuration { get; set; } //TODO: Move to configuration

        /// <summary>
        /// Default timeout value (as seconds) for a job before it's abandoned (<see cref="IsAbandoned"/>).
        /// Default value: 172,800 (2 days).
        /// </summary>
        public static int DefaultTimeout { get; set; } //TODO: Move to configuration

        /// <summary>
        /// Default wait factor for execution failures.
        /// This amount is multiplated by last wait time to calculate next wait time.
        /// Default value: 2.0.
        /// </summary>
        public static double DefaultWaitFactor { get; set; } //TODO: Move to configuration

        public Guid Id { get; set; }

        /// <summary>
        /// Type of the job.
        /// It's AssemblyQualifiedName of job type.
        /// </summary>
        [Required]
        [StringLength(MaxJobTypeLength)]
        public virtual string JobName { get; set; }

        /// <summary>
        /// Job arguments as JSON string.
        /// </summary>
        [Required]
        [MaxLength(MaxJobArgsLength)]
        public virtual string JobArgs { get; set; } //TODO: Consider to conver to byte[]

        /// <summary>
        /// Try count of this job.
        /// A job is re-tried if it fails.
        /// </summary>
        public virtual short TryCount { get; set; }

        /// <summary>
        /// Creation time of this job.
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Next try time of this job.
        /// </summary>
        public virtual DateTime NextTryTime { get; set; }

        /// <summary>
        /// Last try time of this job.
        /// </summary>
        public virtual DateTime? LastTryTime { get; set; }

        /// <summary>
        /// This is true if this job is continously failed and will not be executed again.
        /// </summary>
        public virtual bool IsAbandoned { get; set; }

        /// <summary>
        /// Priority of this job.
        /// </summary>
        public virtual BackgroundJobPriority Priority { get; set; }

        static BackgroundJobInfo()
        {
            DefaultFirstWaitDuration = 60;
            DefaultTimeout = 172800;
            DefaultWaitFactor = 2.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJobInfo"/> class.
        /// </summary>
        public BackgroundJobInfo()
        {
            Priority = BackgroundJobPriority.Normal;
        }

        /// <summary>
        /// Calculates next try time if a job fails.
        /// Returns null if it will not wait anymore and job should be abandoned.
        /// </summary>
        /// <returns></returns>
        public virtual DateTime? CalculateNextTryTime(IClock clock) //TODO: Move to another place to override easier
        {
            var nextWaitDuration = DefaultFirstWaitDuration * (Math.Pow(DefaultWaitFactor, TryCount - 1));
            var nextTryDate = LastTryTime.HasValue
                ? LastTryTime.Value.AddSeconds(nextWaitDuration)
                : clock.Now.AddSeconds(nextWaitDuration);

            if (nextTryDate.Subtract(CreationTime).TotalSeconds > DefaultTimeout)
            {
                return null;
            }

            return nextTryDate;
        }
    }
}