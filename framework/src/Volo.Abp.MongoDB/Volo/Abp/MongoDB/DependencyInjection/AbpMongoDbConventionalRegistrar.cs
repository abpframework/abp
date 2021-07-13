using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MongoDB.DependencyInjection
{
    public class AbpMongoDbConventionalRegistrar : DefaultConventionalRegistrar
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (!typeof(IAbpMongoDbContext).IsAssignableFrom(type) || type == typeof(AbpMongoDbContext))
            {
                return;
            }

            var dependencyAttribute = GetDependencyAttributeOrNull(type);
            var lifeTime = GetLifeTimeOrNull(type, dependencyAttribute);

            if (lifeTime == null)
            {
                return;
            }

            services.Add(ServiceDescriptor.Describe(typeof(IAbpMongoDbContext), type, ServiceLifetime.Transient));
        }
    }
}
