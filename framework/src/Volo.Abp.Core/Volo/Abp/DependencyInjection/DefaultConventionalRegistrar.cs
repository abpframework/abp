using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Volo.Abp.DependencyInjection;

//TODO: Make DefaultConventionalRegistrar extensible, so we can only define GetLifeTimeOrNull to contribute to the convention. This can be more performant!
public class DefaultConventionalRegistrar : ConventionalRegistrarBase
{
    public override void AddType(IServiceCollection services, Type type)
    {
        if (IsConventionalRegistrationDisabled(type))
        {
            return;
        }

        var dependencyAttribute = GetDependencyAttributeOrNull(type);
        var lifeTime = GetLifeTimeOrNull(type, dependencyAttribute);

        if (lifeTime == null)
        {
            return;
        }

        var exposedServiceAndKeyedServiceTypes = GetExposedKeyedServiceTypes(type).Concat(GetExposedServiceTypes(type).Select(t => new ServiceIdentifier(t))).ToList();

        TriggerServiceExposing(services, type, exposedServiceAndKeyedServiceTypes);

        foreach (var exposedServiceType in exposedServiceAndKeyedServiceTypes)
        {
            var allExposingServiceTypes = exposedServiceType.ServiceKey == null
                ? exposedServiceAndKeyedServiceTypes.Where(x => x.ServiceKey == null).ToList()
                : exposedServiceAndKeyedServiceTypes.Where(x => x.ServiceKey?.ToString() == exposedServiceType.ServiceKey?.ToString()).ToList();

            var serviceDescriptor = CreateServiceDescriptor(
                type,
                exposedServiceType.ServiceKey,
                exposedServiceType.ServiceType,
                allExposingServiceTypes,
                lifeTime.Value
            );

            if (dependencyAttribute?.ReplaceServices == true)
            {
                services.Replace(serviceDescriptor);
            }
            else if (dependencyAttribute?.TryRegister == true)
            {
                services.TryAdd(serviceDescriptor);
            }
            else
            {
                services.Add(serviceDescriptor);
            }
        }
    }
}
