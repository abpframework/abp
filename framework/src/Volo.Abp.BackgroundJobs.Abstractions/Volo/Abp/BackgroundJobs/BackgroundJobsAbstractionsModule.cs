﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.BackgroundJobs
{
    [DependsOn(
        typeof(JsonModule)
        )]
    public class BackgroundJobsAbstractionsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            RegisterJobs(context.Services);
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
                    options.AddJob(jobType);
                }
            });
        }
    }
}
