using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobExecuter : IBackgroundJobExecuter, ITransientDependency
    {
        public ILogger<BackgroundJobExecuter> Logger { protected get; set; }

        protected IServiceProvider ServiceProvider { get; }
        protected IBackgroundJobSerializer Serializer { get; }
        protected BackgroundJobOptions Options { get; }

        public BackgroundJobExecuter(
            IServiceProvider serviceProvider,
            IBackgroundJobSerializer serializer,
            IOptions<BackgroundJobOptions> options)
        {
            ServiceProvider = serviceProvider;
            Serializer = serializer;
            Options = options.Value;

            Logger = NullLogger<BackgroundJobExecuter>.Instance;
        }

        public virtual void Execute(JobExecutionContext context)
        {
            //TODO: Refactor (split to multiple methods).

            var jobType = Options.GetJobType(context.JobName);

            using (var scope = ServiceProvider.CreateScope())
            {
                var job = scope.ServiceProvider.GetService(jobType);
                if (job == null)
                {
                    throw new AbpException("The job type is not registered to DI: " + jobType);
                }

                var jobExecuteMethod = job.GetType().GetMethod("Execute");
                Debug.Assert(jobExecuteMethod != null, nameof(jobExecuteMethod) + " != null");
                var argsType = jobExecuteMethod.GetParameters()[0].ParameterType;
                var argsObj = Serializer.Deserialize(context.JobArgs, argsType);

                try
                {
                    jobExecuteMethod.Invoke(job, new[] { argsObj });
                }
                catch (Exception ex)
                {
                    context.Result = JobExecutionResult.Failed;

                    Logger.LogException(ex);

                    //TODO: Somehow trigger an event for the exception (may create an Volo.Abp.ExceptionHandling package)!
                    var backgroundJobException = new BackgroundJobException("A background job execution is failed. See inner exception for details.", ex)
                    {
                        JobName = context.JobName,
                        JobArgs = context.JobArgs
                    };
                }
            }
        }
    }
}