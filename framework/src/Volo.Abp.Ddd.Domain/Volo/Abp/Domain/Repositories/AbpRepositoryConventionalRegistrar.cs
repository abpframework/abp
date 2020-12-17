using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Domain.Repositories
{
    public class AbpRepositoryConventionalRegistrar : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            if (!typeof(IRepository).IsAssignableFrom(type))
            {
                return true;
            }

            return base.IsConventionalRegistrationDisabled(type);
        }

        protected override List<Type> GetExposedServiceTypes(IServiceCollection services, Type type)
        {
            if (services.ExecutePreConfiguredActions<AbpRepositoryConventionalRegistrarOptions>().ExposeRepositoryClasses)
            {
                return base.GetExposedServiceTypes(services, type);
            }

            return base.GetExposedServiceTypes(services, type)
                .Where(x => x.IsInterface)
                .ToList();
        }

        protected override ServiceLifetime? GetServiceLifetimeFromClassHierarchy(Type type)
        {
            return base.GetServiceLifetimeFromClassHierarchy(type) ??
                   ServiceLifetime.Transient;
        }
    }
}
