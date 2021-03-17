using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Reflection;

namespace Volo.Abp.Data
{
    //TODO: Move to Volo.Abp.Data.ObjectExtending namespace at v3.0

    public static class HasExtraPropertiesExtensions
    {
        public static bool HasProperty(this IHasExtraProperties source, string name)
        {
            return source.ExtraProperties.ContainsKey(name);
        }

        public static object GetProperty(this IHasExtraProperties source, string name, object defaultValue = null)
        {
            return source.ExtraProperties?.GetOrDefault(name)
                   ?? defaultValue;
        }

        public static TProperty GetProperty<TProperty>(this IHasExtraProperties source, string name, TProperty defaultValue = default)
        {
            var value = source.GetProperty(name);
            if (value == null)
            {
                return defaultValue;
            }

            if (TypeHelper.IsPrimitiveExtended(typeof(TProperty), includeEnums: true))
            {
                var conversionType = typeof(TProperty);
                if (TypeHelper.IsNullable(conversionType))
                {
                    conversionType = conversionType.GetFirstGenericArgumentIfNullable();
                }

                if (conversionType == typeof(Guid))
                {
                    return (TProperty)TypeDescriptor.GetConverter(conversionType).ConvertFromInvariantString(value.ToString());
                }

                return (TProperty)Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
            }

            throw new AbpException("GetProperty<TProperty> does not support non-primitive types. Use non-generic GetProperty method and handle type casting manually.");
        }

        public static TSource SetProperty<TSource>(
            this TSource source,
            string name,
            object value,
            bool validate = true)
            where TSource : IHasExtraProperties
        {
            if (validate)
            {
                ExtensibleObjectValidator.CheckValue(source, name, value);
            }

            source.ExtraProperties[name] = value;

            return source;
        }

        public static TSource RemoveProperty<TSource>(this TSource source, string name)
            where TSource : IHasExtraProperties
        {
            source.ExtraProperties.Remove(name);
            return source;
        }

        public static TSource SetDefaultsForExtraProperties<TSource>(this TSource source, Type objectType = null)
            where TSource : IHasExtraProperties
        {
            if (objectType == null)
            {
                objectType = typeof(TSource);
            }

            var properties = ObjectExtensionManager.Instance
                .GetProperties(objectType);

            foreach (var property in properties)
            {
                if (source.HasProperty(property.Name))
                {
                    continue;
                }

                source.ExtraProperties[property.Name] = property.GetDefaultValue();
            }

            return source;
        }

        public static void SetDefaultsForExtraProperties(object source, Type objectType)
        {
            if (!(source is IHasExtraProperties))
            {
                throw new ArgumentException($"Given {nameof(source)} object does not implement the {nameof(IHasExtraProperties)} interface!", nameof(source));
            }

            ((IHasExtraProperties)source).SetDefaultsForExtraProperties(objectType);
        }

        public static void SetExtraPropertiesToRegularProperties(this IHasExtraProperties source)
        {
            var properties = source.GetType().GetProperties()
                .Where(x => source.ExtraProperties.ContainsKey(x.Name)
                            && x.GetSetMethod(true) != null)
                .ToList();

            foreach (var property in properties)
            {
                property.SetValue(source, source.ExtraProperties[property.Name]);
                source.RemoveProperty(property.Name);
            }
        }
    }
}
