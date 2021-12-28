using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Volo.Abp;

public static class ObjectHelper
{
    private static readonly ConcurrentDictionary<string, PropertyInfo> CachedObjectProperties =
        new ConcurrentDictionary<string, PropertyInfo>();

    public static void TrySetProperty<TObject, TValue>(
        TObject obj,
        Expression<Func<TObject, TValue>> propertySelector,
        Func<TValue> valueFactory,
        params Type[] ignoreAttributeTypes)
    {
        TrySetProperty(obj, propertySelector, x => valueFactory(), ignoreAttributeTypes);
    }

    public static void TrySetProperty<TObject, TValue>(
        TObject obj,
        Expression<Func<TObject, TValue>> propertySelector,
        Func<TObject, TValue> valueFactory,
        params Type[] ignoreAttributeTypes)
    {
        var cacheKey = $"{obj.GetType().FullName}-" +
                       $"{propertySelector}-" +
                       $"{(ignoreAttributeTypes != null ? "-" + string.Join("-", ignoreAttributeTypes.Select(x => x.FullName)) : "")}";

        var property = CachedObjectProperties.GetOrAdd(cacheKey, () =>
        {
            if (propertySelector.Body.NodeType != ExpressionType.MemberAccess)
            {
                return null;
            }

            var memberExpression = propertySelector.Body.As<MemberExpression>();

            var propertyInfo = obj.GetType().GetProperties().FirstOrDefault(x =>
                x.Name == memberExpression.Member.Name &&
                x.GetSetMethod(true) != null);

            if (propertyInfo == null)
            {
                return null;
            }

            if (ignoreAttributeTypes != null &&
                ignoreAttributeTypes.Any(ignoreAttribute => propertyInfo.IsDefined(ignoreAttribute, true)))
            {
                return null;
            }

            return propertyInfo;
        });

        property?.SetValue(obj, valueFactory(obj));
    }
}
