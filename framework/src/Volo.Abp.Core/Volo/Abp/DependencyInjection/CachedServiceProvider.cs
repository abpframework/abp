using System;
using System.Collections.Generic;

namespace Volo.Abp.DependencyInjection
{
    [ExposeServices(typeof(ICachedServiceProvider))]
    public class CachedServiceProvider : ICachedServiceProvider, IScopedDependency
    {
        protected IServiceProvider ServiceProvider { get; }
        
        protected IDictionary<Type, object> CachedServices { get; }

        public CachedServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            
            CachedServices = new Dictionary<Type, object>
            {
                {typeof(IServiceProvider), serviceProvider}
            };
        }
        
        public object GetService(Type serviceType)
        {
            return CachedServices.GetOrAdd(
                serviceType,
                () => ServiceProvider.GetService(serviceType)
            );
        }
    }
}