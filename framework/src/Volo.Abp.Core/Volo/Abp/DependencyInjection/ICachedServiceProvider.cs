using System;

namespace Volo.Abp.DependencyInjection
{
    /// <summary>
    /// Provides services by caching the resolved services.
    /// It caches all type of services including transients.
    /// This service's lifetime is scoped and it should be used
    /// for a limited scope.
    /// </summary>
    public interface ICachedServiceProvider : IServiceProvider
    {
        
    }
}