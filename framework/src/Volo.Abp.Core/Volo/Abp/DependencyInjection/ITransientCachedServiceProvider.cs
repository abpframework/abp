namespace Volo.Abp.DependencyInjection;

/// <summary>
/// Provides services by caching the resolved services.
/// It caches all type of services including transients.
/// This service's lifetime is transient.
/// <see cref="ICachedServiceProvider"/> for the one with scoped lifetime.
/// </summary>
public interface ITransientCachedServiceProvider : ICachedServiceProviderBase
{

}