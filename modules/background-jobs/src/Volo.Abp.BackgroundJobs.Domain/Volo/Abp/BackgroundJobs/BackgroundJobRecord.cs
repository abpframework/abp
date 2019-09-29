using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobRecord : AggregateRoot<Guid>, IHasCreationTime
    {
        /// <summary>
        /// Type of the job.
        /// It's AssemblyQualifiedName of job type.
        /// </summary>
        public virtual string JobName { get; set; }

        /// <summary>
        /// Job arguments as serialized string.
        /// </summary>
        public virtual string JobArgs { get; set; }

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
        /// This is true if this job is continuously failed and will not be executed again.
        /// </summary>
        public virtual bool IsAbandoned { get; set; }

        /// <summary>
        /// Priority of this job.
        /// </summary>
        public virtual BackgroundJobPriority Priority { get; set; }

        private BackgroundJobRecord()
        {
            
        }

        public BackgroundJobRecord(Guid id)
            : base(id)
        {
            
        }
    }
}
