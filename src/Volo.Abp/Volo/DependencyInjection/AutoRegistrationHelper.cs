using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.ExtensionMethods;

namespace Volo.DependencyInjection
{
    public static class AutoRegistrationHelper
    {
        public static IEnumerable<Type> GetExposedServices(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            var customExposedServices = typeInfo
                .GetCustomAttributes()
                .OfType<IExposedServiceTypesProvider>()
                .SelectMany(p => p.GetExposedServiceTypes())
                .ToList();

            if (customExposedServices.Any())
            {
                return customExposedServices;
            }

            return GetDefaultExposedServices(type);
        }

        public static IEnumerable<Type> GetDefaultExposedServices(Type type, bool includeSelf = true)
        {
            var serviceTypes = new List<Type>();

            if (includeSelf)
            {
                serviceTypes.Add(type);
            }

            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                var interfaceName = interfaceType.Name;

                if (interfaceName.StartsWith("I"))
                {
                    interfaceName = interfaceName.Right(interfaceName.Length - 1);
                }

                if (type.Name.EndsWith(interfaceName))
                {
                    serviceTypes.Add(interfaceType);
                }
            }

            return serviceTypes;
        }
    }
}
