using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Volo.Abp;

public static class ObjectHelper
{
    // Keyed by the object's type itself, since different types can share a full name (such as proxies from different
    // generators), and weakly, so a collectible type can still be unloaded.
    private static readonly ConditionalWeakTable<Type, ConcurrentDictionary<string, PropertyInfo?>> _cachedObjectProperties = new();

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
        params Type[]? ignoreAttributeTypes)
    {
        if (obj == null)
        {
            return;
        }

        var cacheKey = $"{propertySelector}-{(ignoreAttributeTypes != null ? "-" + string.Join("-", ignoreAttributeTypes.Select(x => x.FullName)) : "")}";

        var property = _cachedObjectProperties
            .GetValue(obj.GetType(), _ => new ConcurrentDictionary<string, PropertyInfo?>())
            .GetOrAdd(cacheKey, PropertyFactory);

        property?.SetValue(obj, valueFactory(obj));
        return;

        PropertyInfo? PropertyFactory(string _)
        {
            MemberExpression? memberExpression;
            switch (propertySelector.Body.NodeType)
            {
                case ExpressionType.Convert: {
                    memberExpression = propertySelector.Body.As<UnaryExpression>().Operand as MemberExpression;
                    break;
                }
                case ExpressionType.MemberAccess: {
                    memberExpression = propertySelector.Body.As<MemberExpression>();
                    break;
                }
                default: {
                    return null;
                }
            }

            if (memberExpression == null)
            {
                return null;
            }

            var propertyInfo = obj?.GetType()
                .GetProperties()
                .FirstOrDefault(x => x.Name == memberExpression.Member.Name);

            if (propertyInfo == null)
            {
                return null;
            }

            var propPrivateSetMethod = propertyInfo.GetSetMethod(true);
            if (propPrivateSetMethod == null)
            {
                return null;
            }

            if (ignoreAttributeTypes != null && ignoreAttributeTypes.Any(ignoreAttribute => propertyInfo.IsDefined(ignoreAttribute, true)))
            {
                return null;
            }

            return propertyInfo;
        }
    }
}
