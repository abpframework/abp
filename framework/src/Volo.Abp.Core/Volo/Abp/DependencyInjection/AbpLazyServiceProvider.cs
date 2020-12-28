using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection
{
    public class AbpLazyServiceProvider : IAbpLazyServiceProvider, ITransientDependency
    {
        protected IDictionary<Type, Lazy<object>> CachedTypes { get; set; }

        protected IServiceProvider ServiceProvider { get; set; }

        public AbpLazyServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            CachedTypes = new Dictionary<Type, Lazy<object>>();
        }

        public virtual T LazyGetRequiredService<T>()
        {
            return (T)CachedTypes.GetOrAdd(typeof(T), () => new Lazy<object>(() => (T) ServiceProvider.GetRequiredService(typeof(T)))).Value;
        }

        public virtual object LazyGetRequiredService(Type serviceType)
        {
            return CachedTypes.GetOrAdd(serviceType, () => new Lazy<object>(() => ServiceProvider.GetRequiredService(serviceType))).Value;
        }

        public virtual T LazyGetService<T>()
        {
            return (T)CachedTypes.GetOrAdd(typeof(T), () => new Lazy<object>(() => (T) ServiceProvider.GetService(typeof(T)))).Value;
        }

        public virtual object LazyGetService(Type serviceType)
        {
            return CachedTypes.GetOrAdd(serviceType, () => new Lazy<object>(() => ServiceProvider.GetService(serviceType))).Value;
        }

        public virtual T LazyGetService<T>(T defaultValue)
        {
            return LazyGetService<T>() ?? defaultValue;
        }

        public virtual object LazyGetService(Type serviceType, object defaultValue)
        {
            return LazyGetService(serviceType) ?? defaultValue;
        }

        public virtual object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory)
        {
            return CachedTypes.GetOrAdd(serviceType, () => new Lazy<object>(() => factory(ServiceProvider))).Value;
        }

        public virtual T LazyGetService<T>(Func<IServiceProvider, object> factory)
        {
            return (T)CachedTypes.GetOrAdd(typeof(T), () => new Lazy<object>(() => factory(ServiceProvider))).Value;
        }
    }
}
