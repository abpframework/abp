using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MongoDB.DependencyInjection
{
    public class AbpMongoDbConventionalRegistrar : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !typeof(IAbpMongoDbContext).IsAssignableFrom(type) || type == typeof(AbpMongoDbContext) || base.IsConventionalRegistrationDisabled(type);
        }

        protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }
}
