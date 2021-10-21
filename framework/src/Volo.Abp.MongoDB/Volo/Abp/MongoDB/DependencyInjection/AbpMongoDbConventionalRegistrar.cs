using System;
using JetBrains.Annotations;
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

        protected override ServiceLifetime? GetLifeTimeOrNull(Type type, [CanBeNull] DependencyAttribute dependencyAttribute)
        {
            return dependencyAttribute?.Lifetime ?? GetAbpMongoDbContextLifetime(type);
        }

        protected virtual ServiceLifetime GetAbpMongoDbContextLifetime(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }
}
