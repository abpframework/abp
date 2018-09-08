using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

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
            var handlers = new List<Type>();

            services.OnRegistred(context =>
            {
                if (context.ImplementationType.GetInterfaces().Any(i => typeof(IEventHandler).IsAssignableFrom(i)))
                {
                    handlers.Add(context.ImplementationType);
                }
            });

            services.Configure<EventBusOptions>(options =>
            {
                foreach (var handler in handlers)
                {
                    options.Handlers.AddIfNotContains(handler);
                }
            });
        }
    }
}
