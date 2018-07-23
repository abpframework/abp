using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs
{
    [DependsOn(
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpTimingModule),
        typeof(AbpJsonModule),
        typeof(AbpGuidsModule)
        )]
    public class AbpBackgroundJobsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            RegisterJobs(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpBackgroundJobsModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<BackgroundJobOptions>>().Value;
            if (options.IsJobExecutionEnabled)
            {
                context.ServiceProvider
                    .GetRequiredService<IBackgroundWorkerManager>()
                    .Add(
                        context.ServiceProvider
                            .GetRequiredService<IBackgroundJobWorker>()
                    );
            }
        }

        private static void RegisterJobs(IServiceCollection services)
        {
            var jobTypes = new List<Type>();

            services.OnRegistred(context =>
            {
                if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IBackgroundJob<>)))
                {
                    jobTypes.Add(context.ImplementationType);
                }
            });

            services.Configure<BackgroundJobOptions>(options =>
            {
                foreach (var jobType in jobTypes)
                {
                    var jobArgsType = BackgroundJobHelper.GetJobArgsType(jobType);
                    var jobName = BackgroundJobNameAttribute.GetName(jobArgsType);
                    options.JobTypes[jobName] = jobType;
                }
            });
        }
    }
}