using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Volo.Abp.ObjectMapping;

public static class ObjectMappingHelper
{
    private static readonly ConcurrentDictionary<(Type, Type), (Type sourceArgumentType, Type destinationArgumentType, Type definitionGenericType)?> Cache = new();

    public static bool IsCollectionGenericType<TSource, TDestination>(
        out Type sourceArgumentType,
        out Type destinationArgumentType,
        out Type definitionGenericType)
    {
        var cached = Cache.GetOrAdd((typeof(TSource), typeof(TDestination)), _ => IsCollectionGenericTypeInternal<TSource, TDestination>());
        if (cached == null)
        {
            sourceArgumentType = destinationArgumentType = definitionGenericType = null!;
            return false;
        }

        (sourceArgumentType, destinationArgumentType, definitionGenericType) = cached.Value;
        return true;
    }

    private static (Type, Type, Type)? IsCollectionGenericTypeInternal<TSource, TDestination>()
    {
        if (!IsCollectionGenericTypeInternal(typeof(TSource), out var sourceArgumentType, out _) ||
            !IsCollectionGenericTypeInternal(typeof(TDestination), out var destinationArgumentType, out var definitionGenericType))
        {
            return null;
        }

        return (sourceArgumentType, destinationArgumentType, definitionGenericType);
    }

    private static bool IsCollectionGenericTypeInternal(Type type, out Type elementType, out Type definitionGenericType)
    {
        var supportedCollectionTypes = new[]
        {
            typeof(IEnumerable<>),
            typeof(ICollection<>),
            typeof(Collection<>),
            typeof(IList<>),
            typeof(List<>)
        };

        if (type.IsArray)
        {
            elementType = type.GetElementType()!;
            definitionGenericType = type;
            return true;
        }

        if (type.IsGenericType &&
            supportedCollectionTypes.Contains(type.GetGenericTypeDefinition()) ||
            type.GetInterfaces().Any(i => i.IsGenericType && supportedCollectionTypes.Contains(i.GetGenericTypeDefinition())))
        {
            elementType = type.GetGenericArguments()[0];
            definitionGenericType = type.GetGenericTypeDefinition();
            if (definitionGenericType == typeof(IEnumerable<>) ||
                definitionGenericType == typeof(IList<>))
            {
                definitionGenericType = typeof(List<>);
            }

            if (definitionGenericType == typeof(ICollection<>))
            {
                definitionGenericType = typeof(Collection<>);
            }
            return true;
        }

        elementType = null!;
        definitionGenericType = null!;
        return false;
    }
}
