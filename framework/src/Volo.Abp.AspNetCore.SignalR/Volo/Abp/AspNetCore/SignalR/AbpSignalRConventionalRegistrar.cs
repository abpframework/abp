using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.SignalR
{
    public class AbpSignalRConventionalRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (IsConventionalRegistrationDisabled(type))
            {
                return;
            }

            if (!IsHub(type))
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
        
        private static bool IsHub(Type type)
        {
            return typeof(Hub).IsAssignableFrom(type);
        }
    }
}
