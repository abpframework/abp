using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.EventBus
{
    public class AbpEventBusModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AddEventHandlers(context.Services);
        }

        private static void AddEventHandlers(IServiceCollection services)
        {
            var localHandlers = new List<Type>();
            var distributedHandlers = new List<Type>();

            services.OnRegistred(context =>
            {
                if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IEventHandler<>)))
                {
                    localHandlers.Add(context.ImplementationType);
                }
                //TODO: Distrbiuted event bus is disabled since it's not properly working yet for v0.8 release
                //else if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IDistributedEventHandler<>)))
                //{
                //    distributedHandlers.Add(context.ImplementationType);
                //}
            });

            services.Configure<LocalEventBusOptions>(options =>
            {
                options.Handlers.AddIfNotContains(localHandlers);
            });

            services.Configure<DistributedEventBusOptions>(options =>
            {
                options.Handlers.AddIfNotContains(distributedHandlers);
            });
        }
    }
}
