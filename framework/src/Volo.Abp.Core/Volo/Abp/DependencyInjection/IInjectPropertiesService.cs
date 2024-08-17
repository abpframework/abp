namespace Volo.Abp.DependencyInjection;

public interface IInjectPropertiesService
{
    /// <summary>
    /// Set any properties on <paramref name="instance"/> that can be resolved by IServiceProvider.
    /// </summary>
    TService InjectProperties<TService>(TService instance) where TService : notnull;

    /// <summary>
    /// Set any null-valued properties on <paramref name="instance"/> that can be resolved by the IServiceProvider.
    /// </summary>
    TService InjectUnsetProperties<TService>(TService instance) where TService : notnull;
}
