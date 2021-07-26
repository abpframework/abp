using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;
using Volo.Abp.Quartz.Database.EntityFrameworkCore;

namespace QuartzDatabaseDemo
{

    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpBackgroundJobsQuartzModule),
        typeof(AbpBackgroundWorkersQuartzModule),
        typeof(AbpQuartzDatabaseEntityFrameworkCoreModule)
    )]
    public class QuartzDatabaseSharedDemoModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            //Configure<AbpBackgroundJobQuartzOptions>(options =>
            //{
            //    options.RetryCount = 1;
            //    options.RetryIntervalMillisecond = 1000;
            //});
        }
    }
}
