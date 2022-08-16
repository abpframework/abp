using System;
using System.Linq;
using System.Text.Json.Serialization.Metadata;

namespace Volo.Abp.Json.SystemTextJson.Modifiers;

public class IncludeNonPublicPropertiesModifiers<TClass, TProperty>
    where TClass : class
{
    private string _propertyName;

    public Action<JsonTypeInfo> CreateModifyAction(string propertyName)
    {
        _propertyName = propertyName;
        return Modify;
    }

    public void Modify(JsonTypeInfo jsonTypeInfo)
    {
        if (jsonTypeInfo.Type == typeof(TClass))
        {
            var propertyJsonInfo = jsonTypeInfo.Properties.FirstOrDefault(x => x.Name.Equals(_propertyName, StringComparison.OrdinalIgnoreCase) && x.Set == null);
            if (propertyJsonInfo != null)
            {
                var propertyInfo = typeof(TClass).GetProperty(_propertyName);
                if (propertyInfo != null)
                {
                    var jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(typeof(TProperty), propertyJsonInfo.Name);
                    jsonPropertyInfo.Get = propertyInfo.GetValue;
                    jsonPropertyInfo.Set = propertyInfo.SetValue;
                    jsonTypeInfo.Properties.Remove(propertyJsonInfo);
                    jsonTypeInfo.Properties.Add(jsonPropertyInfo);
                }
            }
        }
    }
}
