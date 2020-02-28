using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.Quartz
{
    public class AbpQuartzConventionalRegistrar : DefaultConventionalRegistrar
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (!typeof(IQuartzBackgroundWorker).IsAssignableFrom(type))
            {
                return;
            }

            var dependencyAttribute = GetDependencyAttributeOrNull(type);
            var lifeTime = GetLifeTimeOrNull(type, dependencyAttribute);

            if (lifeTime == null)
            {
                return;
            }

            services.Add(ServiceDescriptor.Describe(typeof(IQuartzBackgroundWorker), type, lifeTime.Value));
        }
    }
}
