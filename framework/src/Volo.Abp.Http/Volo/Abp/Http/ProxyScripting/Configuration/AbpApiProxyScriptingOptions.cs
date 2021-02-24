using System;
using System.Collections.Generic;
using System.Reflection;

namespace Volo.Abp.Http.ProxyScripting.Configuration
{
    public class AbpApiProxyScriptingOptions
    {
        public IDictionary<string, Type> Generators { get; }

        public static Func<PropertyInfo, string> PropertyNameGenerator { get; set; }

        public AbpApiProxyScriptingOptions()
        {
            Generators = new Dictionary<string, Type>();
            
            PropertyNameGenerator = propertyInfo =>
            {
                var jsonPropertyNameAttribute = propertyInfo.GetSingleAttributeOrNull<System.Text.Json.Serialization.JsonPropertyNameAttribute>(true);

                if (jsonPropertyNameAttribute != null)
                {
                    return jsonPropertyNameAttribute.Name;
                }
                
                var jsonPropertyAttribute = propertyInfo.GetSingleAttributeOrNull<Newtonsoft.Json.JsonPropertyAttribute>(true);

                if (jsonPropertyAttribute != null)
                {
                    return jsonPropertyAttribute.PropertyName;
                }

                return null;
            };
        }
    }
}