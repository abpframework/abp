using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Domain
{
    public class AbpRepositoryConventionalRegistrar : DefaultConventionalRegistrar
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (!typeof(IRepository).IsAssignableFrom(type))
            {
                return;
            }

            var dependencyAttribute = GetDependencyAttributeOrNull(type);
            var lifeTime = GetLifeTimeOrNull(type, dependencyAttribute);

            if (lifeTime != null)
            {
                return;
            }

            var exposedServiceTypes = ExposedServiceExplorer.GetExposedServices(type)
                .Where(x => x.IsInterface).ToList();;

            TriggerServiceExposing(services, type, exposedServiceTypes);

            foreach (var exposedServiceType in exposedServiceTypes)
            {
                var serviceDescriptor = CreateServiceDescriptor(
                    type,
                    exposedServiceType,
                    exposedServiceTypes,
                    ServiceLifetime.Transient
                );

                services.TryAdd(serviceDescriptor);
            }
        }
    }
}
