using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Mapperly;

public class MapperlyAutoObjectMappingProvider<TContext> : MapperlyAutoObjectMappingProvider, IAutoObjectMappingProvider<TContext>
{
    public MapperlyAutoObjectMappingProvider(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }
}

public class MapperlyAutoObjectMappingProvider : IAutoObjectMappingProvider
{
    protected static readonly ConcurrentDictionary<string, Func<object, object, object, object?>> MapCache = new();
    protected static readonly List<MethodInfo> MapMethods = typeof(MapperlyAutoObjectMappingProvider)
        .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        .Where(x => x.Name == nameof(Map)).ToList();

    protected IServiceProvider ServiceProvider { get; }

    public MapperlyAutoObjectMappingProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual TDestination Map<TSource, TDestination>(object source)
    {
        if (TryToMapCollection<TSource, TDestination>((TSource)source, default, out var collectionResult))
        {
            return collectionResult;
        }

        var mapper = ServiceProvider.GetService<IAbpMapperlyMapper<TSource, TDestination>>();
        if (mapper != null)
        {
            mapper.BeforeMap((TSource)source);
            var destination = mapper.Map((TSource)source);
            TryMapExtraProperties(mapper.GetType().GetSingleAttributeOrNull<MapExtraPropertiesAttribute>(), (TSource)source, destination, GetExtraProperties(destination));
            mapper.AfterMap((TSource)source, destination);
            return destination;
        }

        var reverseMapper = ServiceProvider.GetService<IAbpReverseMapperlyMapper<TDestination, TSource>>();
        if (reverseMapper != null)
        {
            reverseMapper.BeforeReverseMap((TSource)source);
            var destination = reverseMapper.ReverseMap((TSource)source);
            TryMapExtraProperties(reverseMapper.GetType().GetSingleAttributeOrNull<MapExtraPropertiesAttribute>(), (TSource)source, destination, GetExtraProperties(destination));
            reverseMapper.AfterReverseMap((TSource)source, destination);
            return destination;
        }

        throw GetNoMapperFoundException<TSource, TDestination>();
    }

    public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (TryToMapCollection<TSource, TDestination>(source, destination, out var collectionResult))
        {
            return collectionResult;
        }

        var mapper = ServiceProvider.GetService<IAbpMapperlyMapper<TSource, TDestination>>();
        if (mapper != null)
        {
            mapper.BeforeMap(source);
            var destinationExtraProperties = GetExtraProperties(destination);
            mapper.Map(source, destination);
            TryMapExtraProperties(mapper.GetType().GetSingleAttributeOrNull<MapExtraPropertiesAttribute>(), source, destination, destinationExtraProperties);
            mapper.AfterMap(source, destination);
            return destination;
        }

        var reverseMapper = ServiceProvider.GetService<IAbpReverseMapperlyMapper<TDestination, TSource>>();
        if (reverseMapper != null)
        {
            reverseMapper.BeforeReverseMap(source);
            var destinationExtraProperties = GetExtraProperties(destination);
            reverseMapper.ReverseMap(source, destination);
            TryMapExtraProperties(reverseMapper.GetType().GetSingleAttributeOrNull<MapExtraPropertiesAttribute>(), source, destination, destinationExtraProperties);
            reverseMapper.AfterReverseMap(source, destination);
            return destination;
        }

        throw GetNoMapperFoundException<TSource, TDestination>();
    }

    protected virtual AbpException GetNoMapperFoundException<TSource, TDestination>()
    {
        var newLine = Environment.NewLine;
        var message = "No object mapping was found for the specified source and destination types." +
                      newLine +
                      newLine +
                      "Mapping attempted:" +
                      newLine +
                      $"{typeof(TSource).Name} -> {typeof(TDestination).Name}" +
                      newLine +
                      $"{typeof(TSource).FullName} -> {typeof(TDestination).FullName}" +
                      newLine +
                      newLine +
                      "How to fix:" +
                      newLine +
                      "Define a mapping class for these types:" +
                      newLine +
                      "   - Use MapperBase<TSource, TDestination> for one-way mapping." +
                      newLine +
                      "   - Use TwoWayMapperBase<TDestination, TSource> for two-way mapping." +
                      newLine +
                      newLine +
                      "For details, see the Mapperly integration document https://abp.io/docs/latest/framework/infrastructure/object-to-object-mapping#mapperly-integration";

        return new AbpException(message);
    }

    protected virtual bool TryToMapCollection<TSource, TDestination>(TSource source, TDestination? destination, out TDestination collectionResult)
    {
        if (!ObjectMappingHelper.IsCollectionGenericType<TSource, TDestination>(out var sourceArgumentType, out var destinationArgumentType, out var definitionGenericType))
        {
            collectionResult = default!;
            return false;
        }

        var mapperType = typeof(IAbpMapperlyMapper<,>).MakeGenericType(sourceArgumentType, destinationArgumentType);
        var mapper = ServiceProvider.GetService(mapperType);
        if (mapper == null)
        {
            mapperType = typeof(IAbpReverseMapperlyMapper<,>).MakeGenericType(destinationArgumentType, sourceArgumentType);
            mapper = ServiceProvider.GetService(mapperType);
            if (mapper == null)
            {
                //skip, no specific mapper
                collectionResult = default!;
                return false;
            }
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
                ? invoker(this, sourceList[i]!, null!)
                : invoker(this, sourceList[i]!, Activator.CreateInstance(destinationArgumentType)!);

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
        var method = !hasDestination
            ? MapMethods.First(x => x.GetParameters().Length == 1).MakeGenericMethod(sourceArgumentType, destinationArgumentType)
            : MapMethods.First(x => x.GetParameters().Length == 2).MakeGenericMethod(sourceArgumentType, destinationArgumentType);
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

    protected virtual ExtraPropertyDictionary GetExtraProperties<TDestination>(TDestination destination)
    {
        var extraProperties = new ExtraPropertyDictionary();
        if (destination is not IHasExtraProperties hasExtraProperties)
        {
            return extraProperties;
        }

        foreach (var property in hasExtraProperties.ExtraProperties)
        {
            extraProperties.Add(property.Key, property.Value);
        }
        return extraProperties;
    }

    protected virtual void TryMapExtraProperties<TSource, TDestination>(MapExtraPropertiesAttribute? mapExtraPropertiesAttribute, TSource source, TDestination destination, ExtraPropertyDictionary destinationExtraProperty)
    {
        if(source is not IHasExtraProperties sourceHasExtraProperties)
        {
            return;
        }

        if (destination is not IHasExtraProperties destinationHasExtraProperties)
        {
            return;
        }

        if (sourceHasExtraProperties.ExtraProperties != null && sourceHasExtraProperties.ExtraProperties ==
            destinationHasExtraProperties.ExtraProperties)
        {
            ObjectHelper.TrySetProperty(destinationHasExtraProperties, x => x.ExtraProperties, () => new ExtraPropertyDictionary(destinationHasExtraProperties.ExtraProperties));;
        }

        if (mapExtraPropertiesAttribute != null)
        {
            MapExtraProperties<TSource, TDestination>(
                sourceHasExtraProperties,
                destinationHasExtraProperties,
                destinationExtraProperty,
                mapExtraPropertiesAttribute.DefinitionChecks,
                mapExtraPropertiesAttribute.IgnoredProperties,
                mapExtraPropertiesAttribute.MapToRegularProperties
            );
        }
    }
    protected virtual void MapExtraProperties<TSource, TDestination>(
        IHasExtraProperties source,
        IHasExtraProperties destination,
        ExtraPropertyDictionary destinationExtraProperty,
        MappingPropertyDefinitionChecks? definitionChecks = null,
        string[]? ignoredProperties = null,
        bool mapToRegularProperties = false)
    {
        var result = destinationExtraProperty.IsNullOrEmpty()
            ? new Dictionary<string, object?>()
            : new Dictionary<string, object?>(destinationExtraProperty);

        if (source.ExtraProperties != null && destination.ExtraProperties != null)
        {
            ExtensibleObjectMapper
                .MapExtraPropertiesTo(
                    typeof(TSource),
                    typeof(TDestination),
                    source.ExtraProperties,
                    result,
                    definitionChecks,
                    ignoredProperties
                );
        }

        ObjectHelper.TrySetProperty(destination, x => x.ExtraProperties, () => new ExtraPropertyDictionary(result));
        if (mapToRegularProperties)
        {
            destination.SetExtraPropertiesToRegularProperties();
        }
    }
}
