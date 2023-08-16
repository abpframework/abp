using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectMapping;

public class DefaultObjectMapper<TContext> : DefaultObjectMapper, IObjectMapper<TContext>
{
    public DefaultObjectMapper(
        IServiceProvider serviceProvider,
        IAutoObjectMappingProvider<TContext> autoObjectMappingProvider
        ) : base(
            serviceProvider,
            autoObjectMappingProvider)
    {

    }
}

public class DefaultObjectMapper : IObjectMapper, ITransientDependency
{
    protected static ConcurrentDictionary<string, MethodInfo> MethodInfoCache { get; } = new ConcurrentDictionary<string, MethodInfo>();

    public IAutoObjectMappingProvider AutoObjectMappingProvider { get; }
    protected IServiceProvider ServiceProvider { get; }

    public DefaultObjectMapper(
        IServiceProvider serviceProvider,
        IAutoObjectMappingProvider autoObjectMappingProvider)
    {
        AutoObjectMappingProvider = autoObjectMappingProvider;
        ServiceProvider = serviceProvider;
    }

    //TODO: It can be slow to always check if service is available. Test it and optimize if necessary.

    public virtual TDestination Map<TSource, TDestination>(TSource source)
    {
        if (source == null)
        {
            return default!;
        }

        using (var scope = ServiceProvider.CreateScope())
        {
            var specificMapper = scope.ServiceProvider.GetService<IObjectMapper<TSource, TDestination>>();
            if (specificMapper != null)
            {
                return specificMapper.Map(source);
            }

            var result = TryToMapCollection<TSource, TDestination>(scope, source, default);
            if (result != null)
            {
                return result;
            }
        }

        if (source is IMapTo<TDestination> mapperSource)
        {
            return mapperSource.MapTo();
        }

        if (typeof(IMapFrom<TSource>).IsAssignableFrom(typeof(TDestination)))
        {
            try
            {
                //TODO: Check if TDestination has a proper constructor which takes TSource
                //TODO: Check if TDestination has an empty constructor (in this case, use MapFrom)

                return (TDestination)Activator.CreateInstance(typeof(TDestination), source)!;
            }
            catch
            {
                //TODO: Remove catch when TODOs are implemented above
            }
        }

        return AutoMap<TSource, TDestination>(source);
    }

    public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source == null)
        {
            return default!;
        }

        using (var scope = ServiceProvider.CreateScope())
        {
            var specificMapper = scope.ServiceProvider.GetService<IObjectMapper<TSource, TDestination>>();
            if (specificMapper != null)
            {
                return specificMapper.Map(source, destination);
            }

            var result = TryToMapCollection(scope, source, destination);
            if (result != null)
            {
                return result;
            }
        }

        if (source is IMapTo<TDestination> mapperSource)
        {
            mapperSource.MapTo(destination);
            return destination;
        }

        if (destination is IMapFrom<TSource> mapperDestination)
        {
            mapperDestination.MapFrom(source);
            return destination;
        }

        return AutoMap(source, destination);
    }

    protected virtual TDestination? TryToMapCollection<TSource, TDestination>(IServiceScope serviceScope, TSource source, TDestination? destination)
    {
        if (!typeof(TSource).IsGenericType || typeof(TSource).GetGenericTypeDefinition() != typeof(ICollection<>) ||
            !typeof(TDestination).IsGenericType || typeof(TDestination).GetGenericTypeDefinition() != typeof(ICollection<>))
        {
            //skip, not a collection
            return default;
        }

        var sourceGenericTypeDefinition = typeof(TSource).GenericTypeArguments[0];
        var destinationGenericTypeDefinition = typeof(TDestination).GenericTypeArguments[0];
        var mapperType = typeof(IObjectMapper<,>).MakeGenericType(sourceGenericTypeDefinition, destinationGenericTypeDefinition);
        var specificMapper = serviceScope.ServiceProvider.GetService(mapperType);
        if (specificMapper == null)
        {
            //skip, no specific mapper
            return default;
        }

        var cacheKey = $"{mapperType.FullName}{(destination == null ? "MapMethodWithSingleParameter" : "MapMethodWithDoubleParameters")}";
        var method = MethodInfoCache.GetOrAdd(cacheKey, _ =>
        {
            return specificMapper.GetType().GetMethods().First(x => x.Name == nameof(IObjectMapper<object, object>.Map) &&
                                                                    x.GetParameters().Length == (destination == null ? 1 : 2));
        });

        var result = Activator.CreateInstance(typeof(Collection<>).MakeGenericType(destinationGenericTypeDefinition))!.As<IList>();
        foreach (var sourceItem in (IEnumerable)source!)
        {
            result.Add(destination == null
                ? method.Invoke(specificMapper, new [] { sourceItem })!
                : method.Invoke(specificMapper, new [] { sourceItem, Activator.CreateInstance(destinationGenericTypeDefinition)! })!);
        }

        return (TDestination)result!;
    }

    protected virtual TDestination AutoMap<TSource, TDestination>(object source)
    {
        return AutoObjectMappingProvider.Map<TSource, TDestination>(source);
    }

    protected virtual TDestination AutoMap<TSource, TDestination>(TSource source, TDestination destination)
    {
        return AutoObjectMappingProvider.Map<TSource, TDestination>(source, destination);
    }
}
