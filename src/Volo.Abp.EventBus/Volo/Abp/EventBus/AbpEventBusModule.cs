using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus
{
    public class AbpEventBusModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            AddEventHandlers(services);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpEventBusModule>();
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
