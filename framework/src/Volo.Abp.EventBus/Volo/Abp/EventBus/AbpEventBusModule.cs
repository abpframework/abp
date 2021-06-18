﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.EventBus.Abstractions;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;

namespace Volo.Abp.EventBus
{
    [DependsOn(
        typeof(AbpEventBusAbstractionsModule),
        typeof(AbpMultiTenancyModule),
        typeof(AbpJsonModule))]
    public class AbpEventBusModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AddEventHandlers(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpEventBusOptions>(options =>
            {
                context.Services.ExecutePreConfiguredActions(options);
            });
        }

        private static void AddEventHandlers(IServiceCollection services)
        {
            var localHandlers = new List<Type>();
            var distributedHandlers = new List<Type>();

            services.OnRegistred(context =>
            {
                if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(ILocalEventHandler<>)))
                {
                    localHandlers.Add(context.ImplementationType);
                }
                else if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IDistributedEventHandler<>)))
                {
                    distributedHandlers.Add(context.ImplementationType);
                }
            });

            services.Configure<AbpLocalEventBusOptions>(options =>
            {
                options.Handlers.AddIfNotContains(localHandlers);
            });

            services.Configure<AbpDistributedEventBusOptions>(options =>
            {
                options.Handlers.AddIfNotContains(distributedHandlers);
            });
        }
    }
}
