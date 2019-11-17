using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobExecuter : IBackgroundJobExecuter, ITransientDependency
    {
        public ILogger<BackgroundJobExecuter> Logger { protected get; set; }

        protected AbpBackgroundJobOptions Options { get; }

        public BackgroundJobExecuter(IOptions<AbpBackgroundJobOptions> options)
        {
            Options = options.Value;

            Logger = NullLogger<BackgroundJobExecuter>.Instance;
        }

        public virtual void Execute(JobExecutionContext context)
        {
            var job = context.ServiceProvider.GetService(context.JobType);
            if (job == null)
            {
                throw new AbpException("The job type is not registered to DI: " + context.JobType);
            }

            var jobExecuteMethod = context.JobType.GetMethod(nameof(IBackgroundJob<object>.Execute));
            if (jobExecuteMethod == null)
            {
                throw new AbpException($"Given job type does not implement {typeof(IBackgroundJob<>).Name}. The job type was: " + context.JobType);
            }

            try
            {
                jobExecuteMethod.Invoke(job, new[] { context.JobArgs });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                throw new BackgroundJobExecutionException("A background job execution is failed. See inner exception for details.", ex)
                {
                    JobType = context.JobType.AssemblyQualifiedName,
                    JobArgs = context.JobArgs
                };
            }
        }
    }
}