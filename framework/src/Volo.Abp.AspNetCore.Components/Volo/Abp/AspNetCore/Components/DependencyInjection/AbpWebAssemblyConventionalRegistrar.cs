using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.DependencyInjection
{
    public class AbpWebAssemblyConventionalRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (IsConventionalRegistrationDisabled(type))
            {
                return;
            }

            if (!IsComponent(type))
            {
                return;
            }

            var serviceTypes = ExposedServiceExplorer.GetExposedServices(type);

            TriggerServiceExposing(services, type, serviceTypes);

            foreach (var serviceType in serviceTypes)
            {
                services.Add(
                    ServiceDescriptor.Describe(
                        serviceType,
                        type,
                        ServiceLifetime.Transient
                    )
                );
            }
        }

        private static bool IsComponent(Type type)
        {
            return typeof(ComponentBase).IsAssignableFrom(type);
        }
    }
}
