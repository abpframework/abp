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
        if (!IsCollectionGenericType<TSource, TDestination>(out var sourceArgumentType, out var destinationArgumentType, out var definitionGenericType))
        {
            return default;
        }

        var mapperType = typeof(IObjectMapper<,>).MakeGenericType(sourceArgumentType, destinationArgumentType);
        var specificMapper = serviceScope.ServiceProvider.GetService(mapperType);
        if (specificMapper == null)
        {
            //skip, no specific mapper
            return default;
        }

        var cacheKey = $"{mapperType.FullName}_{(destination == null ? "MapMethodWithSingleParameter" : "MapMethodWithDoubleParameters")}";
        var method = MethodInfoCache.GetOrAdd(
            cacheKey,
            _ =>
            {
                return specificMapper
                    .GetType()
                    .GetMethods()
                    .First(x =>
                        x.Name == nameof(IObjectMapper<object, object>.Map) &&
                        x.GetParameters().Length == (destination == null ? 1 : 2)
                    );
            }
        );

        var sourceList = source!.As<IList>();
        var result = definitionGenericType.IsGenericType
            ? Activator.CreateInstance(definitionGenericType.MakeGenericType(destinationArgumentType))!.As<IList>()
            : Array.CreateInstance(destinationArgumentType, sourceList.Count);

        if (destination != null && !destination.GetType().IsArray)
        {
            //Clear destination collection if destination not an array, We won't change array just same behavior as AutoMapper.
            destination.As<IList>().Clear();
        }

        for (var i = 0; i < sourceList.Count; i++)
        {
            var invokeResult = destination == null
                ? method.Invoke(specificMapper, new [] { sourceList[i] })!
                : method.Invoke(specificMapper, new [] { sourceList[i], Activator.CreateInstance(destinationArgumentType)! })!;

            if (definitionGenericType.IsGenericType)
            {
                result.Add(invokeResult);
                destination?.As<IList>().Add(invokeResult);
            }
            else
            {
                result[i] = invokeResult;
            }
        }

        if (destination != null && destination.GetType().IsArray)
        {
            //Return the new collection if destination is an array,  We won't change array just same behavior as AutoMapper.
            return (TDestination)result;
        }

        //Return the destination if destination exists. The parameter reference equals with return object.
        return destination ?? (TDestination)result;
    }

    protected virtual bool IsCollectionGenericType<TSource, TDestination>(out Type sourceArgumentType, out Type destinationArgumentType, out Type definitionGenericType)
    {
        sourceArgumentType = default!;
        destinationArgumentType = default!;
        definitionGenericType = default!;

        if ((!typeof(TSource).IsGenericType && !typeof(TSource).IsArray) ||
            (!typeof(TDestination).IsGenericType && !typeof(TDestination).IsArray))
        {
            return false;
        }

        var supportedCollectionTypes = new[]
        {
            typeof(IEnumerable<>),
            typeof(ICollection<>),
            typeof(Collection<>),
            typeof(IList<>),
            typeof(List<>)
        };

        if (typeof(TSource).IsGenericType && supportedCollectionTypes.Any(x => x == typeof(TSource).GetGenericTypeDefinition()))
        {
            sourceArgumentType = typeof(TSource).GenericTypeArguments[0];
        }

        if (typeof(TSource).IsArray)
        {
            sourceArgumentType = typeof(TSource).GetElementType()!;
        }

        if (sourceArgumentType == default!)
        {
            return false;
        }

        definitionGenericType = typeof(List<>);
        if (typeof(TDestination).IsGenericType && supportedCollectionTypes.Any(x => x == typeof(TDestination).GetGenericTypeDefinition()))
        {
            destinationArgumentType = typeof(TDestination).GenericTypeArguments[0];

            if (typeof(TDestination).GetGenericTypeDefinition() == typeof(ICollection<>) ||
                typeof(TDestination).GetGenericTypeDefinition() == typeof(Collection<>))
            {
                definitionGenericType = typeof(Collection<>);
            }
        }

        if (typeof(TDestination).IsArray)
        {
            destinationArgumentType = typeof(TDestination).GetElementType()!;
            definitionGenericType = typeof(Array);
        }

        return destinationArgumentType != default!;
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
