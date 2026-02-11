using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
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
    protected static readonly ConcurrentDictionary<string, Func<object, object, object, object?>> MapCache = new();

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

            if (TryToMapCollection<TSource, TDestination>(scope, source, default, out var collectionResult))
            {
                return collectionResult;
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

            if (TryToMapCollection(scope, source, destination, out var collectionResult))
            {
                return collectionResult;
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

    protected virtual bool TryToMapCollection<TSource, TDestination>(IServiceScope serviceScope, TSource source, TDestination? destination, out TDestination collectionResult)
    {
        if (!ObjectMappingHelper.IsCollectionGenericType<TSource, TDestination>(out var sourceArgumentType, out var destinationArgumentType, out var definitionGenericType))
        {
            collectionResult = default!;
            return false;
        }

        var mapperType = typeof(IObjectMapper<,>).MakeGenericType(sourceArgumentType, destinationArgumentType);
        var specificMapper = serviceScope.ServiceProvider.GetService(mapperType);
        if (specificMapper == null)
        {
            //skip, no specific mapper
            collectionResult = default!;
            return false;
        }

        var invoker = MapCache.GetOrAdd(
            $"{mapperType.FullName}_{(destination == null ? "MapMethodWithSingleParameter" : "MapMethodWithDoubleParameters")}",
            _ => CreateMapDelegate(mapperType, sourceArgumentType, destinationArgumentType, destination != null));

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
                ? invoker(specificMapper, sourceList[i]!, null!)
                : invoker(specificMapper, sourceList[i]!, Activator.CreateInstance(destinationArgumentType)!);

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
            collectionResult = (TDestination)result;
            return true;
        }

        //Return the destination if destination exists. The parameter reference equals with return object.
        collectionResult = destination ?? (TDestination)result;
        return true;
    }

    protected virtual Func<object, object, object, object?> CreateMapDelegate(
        Type mapperType,
        Type sourceArgumentType,
        Type destinationArgumentType,
        bool hasDestination)
    {
        var methods = mapperType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.Name == nameof(IObjectMapper<object, object>.Map))
            .Where(x =>
            {
                var parameters = x.GetParameters();
                if (!hasDestination && parameters.Length != 1 ||
                    hasDestination && parameters.Length != 2 ||
                    parameters[0].ParameterType != sourceArgumentType)
                {
                    return false;
                }

                return !hasDestination || parameters[1].ParameterType == destinationArgumentType;
            })
            .ToList();

        if (methods.Count == 0)
        {
            throw new AbpException($"Could not find a method named '{nameof(IObjectMapper<object, object>.Map)}'" +
                                   $" with parameters({(hasDestination ? sourceArgumentType + ", " + destinationArgumentType : sourceArgumentType.ToString())})" +
                                   $" in the type '{mapperType}'.");
        }

        if (methods.Count > 1)
        {
            throw new AbpException($"Found more than one method named '{nameof(IObjectMapper<object, object>.Map)}'" +
                                   $" with parameters({(hasDestination ? sourceArgumentType + ", " + destinationArgumentType : sourceArgumentType.ToString())})" +
                                   $" in the type '{mapperType}'.");
        }

        var method = methods[0];

        var instanceParam = Expression.Parameter(typeof(object), "mapper");
        var sourceParam = Expression.Parameter(typeof(object), "source");
        var destinationParam = Expression.Parameter(typeof(object), "destination");

        var instanceCast = Expression.Convert(instanceParam, method.DeclaringType!);
        var callParams = new List<Expression>
        {
            Expression.Convert(sourceParam, sourceArgumentType)
        };

        if (hasDestination)
        {
            callParams.Add(Expression.Convert(destinationParam, destinationArgumentType));
        }

        var call = Expression.Call(instanceCast, method, callParams);
        var callConvert = Expression.Convert(call, typeof(object));

        return Expression.Lambda<Func<object, object, object, object?>>(callConvert, instanceParam, sourceParam, destinationParam).Compile();
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
