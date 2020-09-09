using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.EntityFrameworkCore.ValueConverters
{
    public class ExtraPropertiesValueConverter : ValueConverter<Dictionary<string, object>, string>
    {
        public ExtraPropertiesValueConverter(Type entityType)
            : base(
                d => SerializeObject(d, entityType),
                s => DeserializeObject(s, entityType))
        {

        }

        private static string SerializeObject(Dictionary<string, object> extraProperties, Type entityType)
        {
            var copyDictionary = new Dictionary<string, object>(extraProperties);

            if (entityType != null)
            {
                var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
                if (objectExtension != null)
                {
                    foreach (var property in objectExtension.GetProperties())
                    {
                        if (property.IsMappedToFieldForEfCore())
                        {
                            copyDictionary.Remove(property.Name);
                        }
                    }
                }
            }

            return JsonConvert.SerializeObject(copyDictionary, Formatting.None);
        }

        private static Dictionary<string, object> DeserializeObject(string extraPropertiesAsJson, Type entityType)
        {
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(extraPropertiesAsJson);

            if (entityType != null)
            {
                var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
                if (objectExtension != null)
                {
                    foreach (var property in objectExtension.GetProperties())
                    {
                        dictionary[property.Name] = GetNormalizedValue(dictionary, property);
                    }
                }
            }

            return dictionary;
        }

        private static object GetNormalizedValue(
            Dictionary<string, object> dictionary, 
            ObjectExtensionPropertyInfo property)
        {
            var value = dictionary.GetOrDefault(property.Name);
            if (value == null)
            {
                return property.GetDefaultValue();
            }

            try
            {
                if (property.Type.IsEnum)
                {
                    return Enum.Parse(property.Type, value.ToString(), true);
                }

                //return Convert.ChangeType(value, property.Type);
                return value;
            }
            catch
            {
                return value;
            }
        }
    }
}