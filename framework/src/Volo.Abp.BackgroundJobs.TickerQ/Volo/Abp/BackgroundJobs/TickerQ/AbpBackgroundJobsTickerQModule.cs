using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TickerQ.Utilities;
using TickerQ.Utilities.Enums;
using Volo.Abp.Modularity;
using Volo.Abp.TickerQ;

namespace Volo.Abp.BackgroundJobs.TickerQ;

[DependsOn(
    typeof(AbpBackgroundJobsAbstractionsModule),
    typeof(AbpTickerQModule)
)]
public class AbpBackgroundJobsTickerQModule : AbpModule
{
    private static readonly MethodInfo GetTickerFunctionDelegateMethod =
        typeof(AbpBackgroundJobsTickerQModule).GetMethod(nameof(GetTickerFunctionDelegate), BindingFlags.NonPublic | BindingFlags.Static)!;

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var abpBackgroundJobOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundJobOptions>>();
        var abpBackgroundJobsTickerQOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundJobsTickerQOptions>>();
        var abpTickerQFunctionProvider = context.ServiceProvider.GetRequiredService<AbpTickerQFunctionProvider>();
        foreach (var jobConfiguration in abpBackgroundJobOptions.Value.GetJobs())
        {
            var genericMethod = GetTickerFunctionDelegateMethod.MakeGenericMethod(jobConfiguration.ArgsType);
            var tickerFunctionDelegate = (TickerFunctionDelegate)genericMethod.Invoke(null, [jobConfiguration.ArgsType])!;
            var config = abpBackgroundJobsTickerQOptions.Value.GetConfigurationOrNull(jobConfiguration.JobType);
            abpTickerQFunctionProvider.AddFunction(jobConfiguration.JobName, tickerFunctionDelegate, config?.Priority ?? TickerTaskPriority.Normal, config?.MaxConcurrency ?? 0);
            abpTickerQFunctionProvider.RequestTypes.TryAdd(jobConfiguration.JobName, (jobConfiguration.ArgsType.FullName, jobConfiguration.ArgsType)!);
        }
    }

    private static TickerFunctionDelegate GetTickerFunctionDelegate<TArgs>(Type argsType)
    {
        return async (cancellationToken, serviceProvider, context) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AbpBackgroundJobOptions>>().Value;
            if (!options.IsJobExecutionEnabled)
            {
                throw new AbpException(
                    "Background job execution is disabled. " +
                    "This method should not be called! " +
                    "If you want to enable the background job execution, " +
                    $"set {nameof(AbpBackgroundJobOptions)}.{nameof(AbpBackgroundJobOptions.IsJobExecutionEnabled)} to true! " +
                    "If you've intentionally disabled job execution and this seems a bug, please report it."
                );
            }

            using (var scope = serviceProvider.CreateScope())
            {
                var jobExecuter = serviceProvider.GetRequiredService<IBackgroundJobExecuter>();
                var args = await TickerRequestProvider.GetRequestAsync<TArgs>(context, cancellationToken);
                var jobType = options.GetJob(typeof(TArgs)).JobType;
                var jobExecutionContext = new JobExecutionContext(scope.ServiceProvider, jobType, args!, cancellationToken: cancellationToken);
                await jobExecuter.ExecuteAsync(jobExecutionContext);
            }
        };
    }
}
