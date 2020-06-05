using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;

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

        public virtual async Task ExecuteAsync(JobExecutionContext context)
        {
            var job = context.ServiceProvider.GetService(context.JobType);
            if (job == null)
            {
                throw new AbpException("The job type is not registered to DI: " + context.JobType);
            }

            var jobExecuteMethod = context.JobType.GetMethod(nameof(IBackgroundJob<object>.Execute)) ?? 
                                   context.JobType.GetMethod(nameof(IAsyncBackgroundJob<object>.ExecuteAsync));
            if (jobExecuteMethod == null)
            {
                throw new AbpException($"Given job type does not implement {typeof(IBackgroundJob<>).Name} or {typeof(IAsyncBackgroundJob<>).Name}. " +
                                       "The job type was: " + context.JobType);
            }

            try
            {
                if (jobExecuteMethod.Name == nameof(IAsyncBackgroundJob<object>.ExecuteAsync))
                {
                    await ((Task) jobExecuteMethod.Invoke(job, new[] {context.JobArgs}));
                }
                else
                {
                    jobExecuteMethod.Invoke(job, new[] { context.JobArgs });
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                await context.ServiceProvider
                    .GetRequiredService<IExceptionNotifier>()
                    .NotifyAsync(new ExceptionNotificationContext(ex));

                throw new BackgroundJobExecutionException("A background job execution is failed. See inner exception for details.", ex)
                {
                    JobType = context.JobType.AssemblyQualifiedName,
                    JobArgs = context.JobArgs
                };
            }
        }
    }
}