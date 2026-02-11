using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.DependencyInjection;

public static class ExposedServiceExplorer
{
    private static readonly ExposeServicesAttribute DefaultExposeServicesAttribute =
        new ExposeServicesAttribute
        {
            IncludeDefaults = true,
            IncludeSelf = true
        };

    public static List<Type> GetExposedServices(Type type)
    {
        var exposedServiceTypesProviders = type
            .GetCustomAttributes(true)
            .OfType<IExposedServiceTypesProvider>()
            .ToList();

        if (exposedServiceTypesProviders.IsNullOrEmpty() && type.GetCustomAttributes(true).OfType<IExposedKeyedServiceTypesProvider>().Any())
        {
            // If there is any IExposedKeyedServiceTypesProvider but no IExposedServiceTypesProvider, we will not expose the default services.
            return Array.Empty<Type>().ToList();
        }

        return exposedServiceTypesProviders
            .DefaultIfEmpty(DefaultExposeServicesAttribute)
            .SelectMany(p => p.GetExposedServiceTypes(type))
            .Distinct()
            .ToList();
    }

    public static List<ServiceIdentifier> GetExposedKeyedServices(Type type)
    {
        return type
            .GetCustomAttributes(true)
            .OfType<IExposedKeyedServiceTypesProvider>()
            .SelectMany(p => p.GetExposedServiceTypes(type))
            .Distinct()
            .ToList();
    }
}
