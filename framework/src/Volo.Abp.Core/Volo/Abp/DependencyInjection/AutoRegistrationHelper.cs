using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection
{
    public static class AutoRegistrationHelper
    {
        public static List<Type> GetExposedServices(IServiceCollection services, Type type)
        {
            var typeInfo = type.GetTypeInfo();

            var customExposedServices = typeInfo
                .GetCustomAttributes()
                .OfType<IExposedServiceTypesProvider>()
                .SelectMany(p => p.GetExposedServiceTypes(type))
                .ToList();

            if (customExposedServices.Any())
            {
                return customExposedServices;
            }

            return GetDefaultExposedServices(type);
        }

        private static List<Type> GetDefaultExposedServices(Type type)
        {
            var serviceTypes = new List<Type>();

            serviceTypes.Add(type);

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
