using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.DependencyInjection
{
    public class AbpWebAssemblyConventionalRegistrar : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !IsComponent(type) || base.IsConventionalRegistrationDisabled(type);
        }

        private static bool IsComponent(Type type)
        {
            return typeof(ComponentBase).IsAssignableFrom(type);
        }

        protected override ServiceLifetime? GetLifeTimeOrNull(Type type, [CanBeNull] DependencyAttribute dependencyAttribute)
        {
            return dependencyAttribute?.Lifetime ?? GetWebAssemblyServiceLifetime(type);
        }

        protected virtual ServiceLifetime GetWebAssemblyServiceLifetime(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }
}
