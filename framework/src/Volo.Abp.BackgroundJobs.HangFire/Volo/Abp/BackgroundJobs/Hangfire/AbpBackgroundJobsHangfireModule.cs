﻿using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.Hangfire
{
    [DependsOn(
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpHangfireModule)
    )]
    public class AbpBackgroundJobsHangfireModule : AbpModule
    {
        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundJobOptions>>().Value;
            if (!options.IsJobExecutionEnabled)
            {
                var hangfireOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpHangfireOptions>>().Value;
                hangfireOptions.BackgroundJobServerFactory = CreateOnlyEnqueueJobServer;
            }
        }

        private BackgroundJobServer CreateOnlyEnqueueJobServer(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<JobStorage>();
            return null;
        }
    }
}