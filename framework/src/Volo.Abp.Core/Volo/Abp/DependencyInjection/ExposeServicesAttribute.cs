using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Volo.Abp.DependencyInjection
{
    public class ExposeServicesAttribute : Attribute, IExposedServiceTypesProvider
    {
        public Type[] ExposedServiceTypes { get; }
        
        public ExposeServicesAttribute(params Type[] exposedServiceTypes)
        {
            ExposedServiceTypes = exposedServiceTypes ?? new Type[0];
        }

        public Type[] GetExposedServiceTypes(Type targetType)
        {
            if (ExposedServiceTypes.Any())
            {
                return ExposedServiceTypes;
            }

            return GetDefaultExposedServices(targetType).ToArray();
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