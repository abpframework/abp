using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Domain.Repositories
{
    public class AbpRepositoryConventionalRegistrar : DefaultConventionalRegistrar
    {
        public static bool ExposeRepositoryClasses { get; set; }

        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            if (!typeof(IRepository).IsAssignableFrom(type))
            {
                return true;
            }

            return base.IsConventionalRegistrationDisabled(type);
        }

        protected override List<Type> GetExposedServiceTypes(Type type)
        {
            if (ExposeRepositoryClasses)
            {
                return base.GetExposedServiceTypes(type);
            }

            return base.GetExposedServiceTypes(type)
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
