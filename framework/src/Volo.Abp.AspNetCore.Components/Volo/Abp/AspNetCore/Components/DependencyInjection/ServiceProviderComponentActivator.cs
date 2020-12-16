using System;
using Microsoft.AspNetCore.Components;

namespace Volo.Abp.AspNetCore.Components.DependencyInjection
{
    public class ServiceProviderComponentActivator : IComponentActivator
    {
        public IServiceProvider ServiceProvider { get; }

        public ServiceProviderComponentActivator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IComponent CreateInstance(Type componentType)
        {
            var instance = ServiceProvider.GetService(componentType);

            if (instance == null)
            {
                instance = Activator.CreateInstance(componentType);
            }

            if (!(instance is IComponent component))
            {
                throw new ArgumentException($"The type {componentType.FullName} does not implement {nameof(IComponent)}.", nameof(componentType));
            }

            return component;
        }
    }
}
