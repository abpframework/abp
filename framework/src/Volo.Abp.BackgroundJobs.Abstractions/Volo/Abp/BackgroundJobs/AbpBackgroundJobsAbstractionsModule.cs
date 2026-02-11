using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;

namespace Volo.Abp.BackgroundJobs;

[DependsOn(
    typeof(AbpJsonModule),
    typeof(AbpMultiTenancyAbstractionsModule)
    )]
public class AbpBackgroundJobsAbstractionsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        RegisterJobs(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.IsJobExecutionEnabled = false;
            });
        }
    }

    private static void RegisterJobs(IServiceCollection services)
    {
        var jobTypes = new List<Type>();

        services.OnRegistered(context =>
        {
            if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IBackgroundJob<>)) ||
                ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IAsyncBackgroundJob<>)))
            {
                jobTypes.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpBackgroundJobOptions>(options =>
        {
            foreach (var jobType in jobTypes)
            {
                options.AddJob(jobType);
            }
        });
    }
}
