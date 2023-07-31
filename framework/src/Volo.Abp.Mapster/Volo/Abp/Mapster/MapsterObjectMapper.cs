using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Mapster;

public class MapsterObjectMapper : IObjectMapper, ITransientDependency
{
    public MapsterObjectMapper(IServiceProvider serviceProvider,
        IAutoObjectMappingProvider autoObjectMappingProvider)
    {
        ServiceProvider = serviceProvider;
        AutoObjectMappingProvider = autoObjectMappingProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public IAutoObjectMappingProvider AutoObjectMappingProvider { get; }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        if (source is null)
        {
            return default!;
        }

        var result = TryMapWithSpecificMapper<TSource, TDestination>(source);
        return result ?? AutoObjectMappingProvider.Map<TSource, TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source is null)
        {
            return default!;
        }

        var result = TryMapWithSpecificMapper<TSource, TDestination>(source, destination);
        return result ?? AutoObjectMappingProvider.Map<TSource, TDestination>(source, destination);
    }

    private TDestination? TryMapWithSpecificMapper<TSource, TDestination>(TSource source, TDestination destination)
    {
        var mapper = FindSpecificMapper<TSource, TDestination>();
        return mapper is null ? default : mapper.Map(source, destination);
    }

    private TDestination? TryMapWithSpecificMapper<TSource, TDestination>(TSource source)
    {
        var mapper = FindSpecificMapper<TSource, TDestination>();
        return mapper is null ? default : mapper.Map(source);
    }

    private IObjectMapper<TSource, TDestination>? FindSpecificMapper<TSource, TDestination>()
    {
        using var scope = ServiceProvider.CreateScope();
        return scope.ServiceProvider.GetService<IObjectMapper<TSource, TDestination>>();
    }
}