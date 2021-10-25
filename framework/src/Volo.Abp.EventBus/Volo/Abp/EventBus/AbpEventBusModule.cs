using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Abstractions;
using Volo.Abp.EventBus.Boxes;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;

namespace Volo.Abp.EventBus
{
    [DependsOn(
        typeof(AbpEventBusAbstractionsModule),
        typeof(AbpMultiTenancyModule),
        typeof(AbpJsonModule),
        typeof(AbpGuidsModule),
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpDistributedLockingAbstractionsModule)
        )]
    public class AbpEventBusModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AddEventHandlers(context.Services);
        }
        
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.AddBackgroundWorker<OutboxSenderManager>();
            context.AddBackgroundWorker<InboxProcessManager>();
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
