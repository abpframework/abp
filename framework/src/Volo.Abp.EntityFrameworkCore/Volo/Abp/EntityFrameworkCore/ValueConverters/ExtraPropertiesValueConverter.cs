using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Volo.Abp.EntityFrameworkCore.Extensions;

namespace Volo.Abp.EntityFrameworkCore.ValueConverters
{
    public class ExtraPropertiesValueConverter : ValueConverter<Dictionary<string, object>, string>
    {
        public ExtraPropertiesValueConverter(Type entityType)
            : base(
                d => SerializeObject(d, entityType),
                s => DeserializeObject(s))
        {

        }

        private static string SerializeObject(Dictionary<string, object> extraProperties, Type entityType)
        {
            var copyDictionary = new Dictionary<string, object>(extraProperties);

            if (entityType != null)
            {
                var propertyNames = EntityExtensionManager.GetPropertyNames(entityType);

                foreach (var propertyName in propertyNames)
                {
                    copyDictionary.Remove(propertyName);
                }
            }

            return JsonConvert.SerializeObject(copyDictionary, Formatting.None);
        }

        private static Dictionary<string, object> DeserializeObject(string extraPropertiesAsJson)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(extraPropertiesAsJson);
        }
    }
}