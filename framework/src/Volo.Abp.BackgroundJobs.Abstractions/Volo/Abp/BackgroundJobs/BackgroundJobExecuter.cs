using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobExecuter : IBackgroundJobExecuter, ITransientDependency
{
    public ILogger<BackgroundJobExecuter> Logger { protected get; set; }

    protected AbpBackgroundJobOptions Options { get; }

    protected ICurrentTenant CurrentTenant { get; }

    public BackgroundJobExecuter(IOptions<AbpBackgroundJobOptions> options, ICurrentTenant currentTenant)
    {
        CurrentTenant = currentTenant;
        Options = options.Value;

        Logger = NullLogger<BackgroundJobExecuter>.Instance;
    }

    public virtual async Task ExecuteAsync(JobExecutionContext context)
    {
        if (context.JobName != null)
        {
            var jobConfig = Options.GetJobOrNull(context.JobName);
            if (jobConfig?.DynamicHandler != null)
            {
                await ExecuteDynamicHandlerAsync(context, jobConfig);
                return;
            }
        }

        await ExecuteTypedHandlerAsync(context);
    }

    protected virtual async Task ExecuteDynamicHandlerAsync(JobExecutionContext context, BackgroundJobConfiguration jobConfig)
    {
        try
        {
            var cancellationTokenProvider =
                context.ServiceProvider.GetRequiredService<ICancellationTokenProvider>();

            using (cancellationTokenProvider.Use(context.CancellationToken))
            {
                var dictArgs = EnsureDictionaryArgs(context.JobArgs);
                var dynamicContext = new DynamicBackgroundJobContext(
                    context.ServiceProvider,
                    dictArgs,
                    context.CancellationToken
                );

                await jobConfig.DynamicHandler!(dynamicContext);
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
                JobType = context.JobName!,
                JobArgs = context.JobArgs
            };
        }
    }

    protected virtual async Task ExecuteTypedHandlerAsync(JobExecutionContext context)
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
            using (CurrentTenant.Change(GetJobArgsTenantId(context.JobArgs)))
            {
                var cancellationTokenProvider =
                    context.ServiceProvider.GetRequiredService<ICancellationTokenProvider>();

                using (cancellationTokenProvider.Use(context.CancellationToken))
                {
                    if (jobExecuteMethod.Name == nameof(IAsyncBackgroundJob<object>.ExecuteAsync))
                    {
                        await ((Task)jobExecuteMethod.Invoke(job, [context.JobArgs])!);
                    }
                    else
                    {
                        jobExecuteMethod.Invoke(job, [context.JobArgs]);
                    }
                }
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
                JobType = context.JobType.AssemblyQualifiedName!,
                JobArgs = context.JobArgs
            };
        }
    }

    protected virtual Dictionary<string, object> EnsureDictionaryArgs(object jobArgs)
    {
        if (jobArgs is Dictionary<string, object> dict)
        {
            return dict;
        }

        if (jobArgs is JsonElement jsonElement)
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonElement.GetRawText())
                   ?? new Dictionary<string, object>();
        }

        var json = JsonSerializer.Serialize(jobArgs);
        return JsonSerializer.Deserialize<Dictionary<string, object>>(json)
               ?? new Dictionary<string, object>();
    }

    protected virtual Guid? GetJobArgsTenantId(object jobArgs)
    {
        return jobArgs switch
        {
            IMultiTenant multiTenantJobArgs => multiTenantJobArgs.TenantId,
            _ => CurrentTenant.Id
        };
    }
}
