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
using Volo.Abp.Reflection;

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

    protected IServiceProvider ServiceProvider { get; }

    public MapperlyAutoObjectMappingProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual TDestination Map<TSource, TDestination>(object source)
    {
        var mapper = ServiceProvider.GetService<IAbpMapperlyMapper<TSource, TDestination>>();
        if (mapper != null)
        {
            var beforeMapAttributes = mapper.GetType().GetCustomAttributes(typeof(BeforeMap<>)).Where(x => typeof(TSource).IsAssignableFrom(x.GetType().GetGenericArguments().FirstOrDefault()))
                .ToList();
            var beforeMapAttributeExecuteMethod = typeof(BeforeMap<>).GetMethod(nameof(BeforeMap<object>.Execute),
                BindingFlags.Instance | BindingFlags.Public);
            if (beforeMapAttributeExecuteMethod != null)
            {
                foreach (var beforeMapAttribute in beforeMapAttributes)
                {
                    var executeMethod = beforeMapAttributeExecuteMethod.MakeGenericMethod(typeof(TSource));
                    executeMethod.Invoke(beforeMapAttribute, [source]);
                }
            }
            mapper.BeforeMap((TSource)source);
            var afterMapAttributes = mapper.GetType().GetCustomAttributes(typeof(AfterMap<,>)).Where(x => typeof(TSource).IsAssignableFrom(x.GetType().GetGenericArguments().FirstOrDefault()) &&
                typeof(TDestination).IsAssignableFrom(x.GetType().GetGenericArguments().LastOrDefault())).ToArray();
            var afterMapAttributesWithContext = afterMapAttributes
                .Where(x => typeof(AfterMap<, ,>).IsInstanceOfType(x))
                .ToList();
            var contexts = new List<object?>();
            var createContextMethod = typeof(AfterMap<, ,>).GetMethod(nameof(AfterMap<object, object, object>.CreateContext),
                BindingFlags.Instance | BindingFlags.Public);
            if (createContextMethod != null)
            {
                foreach (var afterMapAttribute in afterMapAttributesWithContext)
                {
                    var context = createContextMethod.Invoke(afterMapAttribute, [source]);
                    contexts.Add(context);
                }
            }
            var destination = mapper.Map((TSource)source);
            TryMapExtraProperties(mapper.GetType().GetSingleAttributeOrNull<MapExtraPropertiesAttribute>(), (TSource)source, destination, new ExtraPropertyDictionary());
            mapper.AfterMap((TSource)source, destination);
            
            var afterMapExecuteMethod = typeof(AfterMap<, ,>).GetMethod(nameof(AfterMap<object, object, object>.Execute),
                BindingFlags.Instance | BindingFlags.Public);
            if (afterMapExecuteMethod != null)
            {
                for (var i = 0; i < afterMapAttributesWithContext.Count; i++)
                {
                    var afterMapAttribute = afterMapAttributesWithContext[i];
                    var executeMethod = afterMapExecuteMethod.MakeGenericMethod(typeof(TSource), typeof(TDestination));
                    executeMethod.Invoke(afterMapAttribute, [source, destination, contexts[i]]);
                }
            }
            
            var afterMapAttributesWithoutContext = afterMapAttributes
                .Where(x => !typeof(AfterMap<, ,>).IsInstanceOfType(x))
                .ToList();
            
            var afterMapExecuteMethodWithoutContext = typeof(AfterMap<,>).GetMethod(nameof(AfterMap<object, object>.Execute),
                BindingFlags.Instance | BindingFlags.Public);
            if (afterMapExecuteMethodWithoutContext != null)
            {
                foreach (var afterMapAttribute in afterMapAttributesWithoutContext)
                {
                    var executeMethod = afterMapExecuteMethodWithoutContext.MakeGenericMethod(typeof(TSource), typeof(TDestination));
                    executeMethod.Invoke(afterMapAttribute, [source, destination]);
                }
            }
         
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
        
        if (TryToMapCollection<TSource, TDestination>((TSource)source, default, out var collectionResult))
        {
            return collectionResult;
        }

        throw new AbpException($"No {TypeHelper.GetFullNameHandlingNullableAndGenerics(typeof(IAbpMapperlyMapper<TSource, TDestination>))} or" +
                               $" {TypeHelper.GetFullNameHandlingNullableAndGenerics(typeof(IAbpReverseMapperlyMapper<TSource, TDestination>))} was found");
    }

    public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
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
        
        if (TryToMapCollection<TSource, TDestination>(source, destination, out var collectionResult))
        {
            return collectionResult;
        }

        throw new AbpException($"No {TypeHelper.GetFullNameHandlingNullableAndGenerics(typeof(IAbpMapperlyMapper<TSource, TDestination>))} or" +
                               $" {TypeHelper.GetFullNameHandlingNullableAndGenerics(typeof(IAbpReverseMapperlyMapper<TSource, TDestination>))} was found");
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
        var methods = typeof(MapperlyAutoObjectMappingProvider)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.Name == nameof(Map))
            .Where(x =>
            {
                var parameters = x.GetParameters();
                return (hasDestination || parameters.Length == 1) &&
                       (!hasDestination || parameters.Length == 2);
            })
            .ToList();

        if (methods.Count == 0)
        {
            throw new AbpException($"Could not find a method named '{nameof(Map)}'" +
                                   $" with parameters({(hasDestination ? sourceArgumentType + ", " + destinationArgumentType : sourceArgumentType.ToString())})" +
                                   $" in the type '{mapperType}'.");
        }

        if (methods.Count > 1)
        {
            throw new AbpException($"Found more than one method named '{nameof(Map)}'" +
                                   $" with parameters({(hasDestination ? sourceArgumentType + ", " + destinationArgumentType : sourceArgumentType.ToString())})" +
                                   $" in the type '{mapperType}'.");
        }

        var method = methods[0].MakeGenericMethod(sourceArgumentType, destinationArgumentType);

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

    protected virtual void TryMapExtraProperties<TSource, TDestination>(MapExtraPropertiesAttribute? mapExtraPropertiesAttribute, TSource source, TDestination destination, ExtraPropertyDictionary destinationExtraProperty)
    {
        if (mapExtraPropertiesAttribute != null &&
            typeof(IHasExtraProperties).IsAssignableFrom(typeof(TDestination)) &&
            typeof(IHasExtraProperties).IsAssignableFrom(typeof(TSource)))
        {
            MapExtraProperties<TSource, TDestination>(
                source!.As<IHasExtraProperties>(),
                destination!.As<IHasExtraProperties>(),
                destinationExtraProperty,
                mapExtraPropertiesAttribute.DefinitionChecks,
                mapExtraPropertiesAttribute.IgnoredProperties,
                mapExtraPropertiesAttribute.MapToRegularProperties
            );
        }
    }
}
