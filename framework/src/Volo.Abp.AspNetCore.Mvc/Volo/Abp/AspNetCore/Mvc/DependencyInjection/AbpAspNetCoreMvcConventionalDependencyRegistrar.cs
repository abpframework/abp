using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
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

            var serviceTypes = ExposedServiceExplorer.GetExposedServices(type);

            TriggerServiceExposing(services, type, serviceTypes);

            foreach (var serviceType in serviceTypes)
            {
                var serviceDescriptor = ServiceDescriptor.Describe(serviceType, type, lifeTime);
                services.Add(serviceDescriptor);
            }
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
