using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.Quartz
{
    public class AbpQuartzConventionalRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (!typeof(IQuartzBackgroundWorker).IsAssignableFrom(type))
            {
                return;
            }

            services.AddTransient(typeof(IQuartzBackgroundWorker), type);
            services.AddTransient(type);
        }
    }
}
