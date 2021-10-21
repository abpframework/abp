using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Domain.Repositories
{
    /* Repositories are not injected by class by default.
     * This class specializes repository registration to apply this rule.
     */
    public class AbpRepositoryConventionalRegistrar : DefaultConventionalRegistrar
    {
        public static bool ExposeRepositoryClasses { get; set; }

        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !typeof(IRepository).IsAssignableFrom(type) || base.IsConventionalRegistrationDisabled(type);
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

        protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }
}
