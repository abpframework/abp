using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.SignalR
{
    public class AbpSignalRConventionalRegistrar : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !IsHub(type) || base.IsConventionalRegistrationDisabled(type);
        }

        private static bool IsHub(Type type)
        {
            return typeof(Hub).IsAssignableFrom(type);
        }

        protected override ServiceLifetime? GetLifeTimeOrNull(Type type, [CanBeNull] DependencyAttribute dependencyAttribute)
        {
            return dependencyAttribute?.Lifetime ?? GetSignalRServiceLifetime(type);
        }

        protected virtual ServiceLifetime GetSignalRServiceLifetime(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }
}
