using System;
using System.Reflection;

namespace Volo.Abp.Http.ProxyScripting.Configuration;

public static class AbpApiProxyScriptingConfiguration
{
    public static Func<PropertyInfo, string?> PropertyNameGenerator { get; set; }

    static AbpApiProxyScriptingConfiguration()
    {
        PropertyNameGenerator = propertyInfo =>
            propertyInfo.GetSingleAttributeOrNull<System.Text.Json.Serialization.JsonPropertyNameAttribute>()?.Name;
    }
}
