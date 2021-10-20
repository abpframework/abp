using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.DependencyInjection
{
    public class AbpAspNetCoreMvcConventionalRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            if (IsConventionalRegistrationDisabled(type))
            {
                return;
            }

            if (!IsMvcService(type))
            {
                return;
            }

            var lifeTime = GetMvcServiceLifetime(type);
            var dependencyAttribute = GetDependencyAttributeOrNull(type);

            var exposedServiceTypes = GetExposedServiceTypes(type);

            TriggerServiceExposing(services, type, exposedServiceTypes);

            foreach (var exposedServiceType in exposedServiceTypes)
            {
                var serviceDescriptor = CreateServiceDescriptor(
                    type,
                    exposedServiceType,
                    exposedServiceTypes,
                    lifeTime
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
        
        protected virtual DependencyAttribute GetDependencyAttributeOrNull(Type type)
        {
            return type.GetCustomAttribute<DependencyAttribute>(true);
        }
        
        protected virtual List<Type> GetExposedServiceTypes(Type type)
        {
            return ExposedServiceExplorer.GetExposedServices(type);
        }
        
        protected virtual ServiceDescriptor CreateServiceDescriptor(
            Type implementationType,
            Type exposingServiceType,
            List<Type> allExposingServiceTypes,
            ServiceLifetime lifeTime)
        {
            return ServiceDescriptor.Describe(
                exposingServiceType,
                implementationType,
                lifeTime
            );
        }

        protected virtual bool IsMvcService(Type type)
        {
            return IsController(type) ||
                   IsPageModel(type) ||
                   IsViewComponent(type);
        }

        protected virtual ServiceLifetime GetMvcServiceLifetime(Type type)
        {
            return ServiceLifetime.Transient;
        }

        private static bool IsPageModel(Type type)
        {
            return typeof(PageModel).IsAssignableFrom(type) || type.IsDefined(typeof(PageModelAttribute), true);
        }

        private static bool IsController(Type type)
        {
            return typeof(Controller).IsAssignableFrom(type) || type.IsDefined(typeof(ControllerAttribute), true);
        }

        private static bool IsViewComponent(Type type)
        {
            return typeof(ViewComponent).IsAssignableFrom(type) || type.IsDefined(typeof(ViewComponentAttribute), true);
        }
    }
}
