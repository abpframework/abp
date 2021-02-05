using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection
{
    public class AbpLazyServiceProvider : IAbpLazyServiceProvider, ITransientDependency
    {
        protected IDictionary<Type, object> CachedServices { get; set; }

        protected IServiceProvider ServiceProvider { get; set; }

        public AbpLazyServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            CachedServices = new Dictionary<Type, object>();
        }

        public virtual T LazyGetRequiredService<T>()
        {
            return (T) LazyGetRequiredService(typeof(T));
        }

        public virtual object LazyGetRequiredService(Type serviceType)
        {
            return CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetRequiredService(serviceType));
        }

        public virtual T LazyGetService<T>()
        {
            return (T) LazyGetService(typeof(T));
        }

        public virtual object LazyGetService(Type serviceType)
        {
            return CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetService(serviceType));
        }

        public virtual T LazyGetService<T>(T defaultValue)
        {
            return (T) LazyGetService(typeof(T), defaultValue);
        }

        public virtual object LazyGetService(Type serviceType, object defaultValue)
        {
            return LazyGetService(serviceType) ?? defaultValue;
        }

        public virtual T LazyGetService<T>(Func<IServiceProvider, object> factory)
        {
            return (T) LazyGetService(typeof(T), factory);
        }

        public virtual object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory)
        {
            return CachedServices.GetOrAdd(serviceType, () => factory(ServiceProvider));
        }
    }
}
