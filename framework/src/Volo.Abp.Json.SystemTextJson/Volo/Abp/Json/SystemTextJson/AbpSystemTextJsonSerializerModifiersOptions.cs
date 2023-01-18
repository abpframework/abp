using System;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using Volo.Abp.Json.SystemTextJson.Modifiers;


namespace Volo.Abp.Json.SystemTextJson;

public class AbpSystemTextJsonSerializerModifiersOptions
{
    public List<Action<JsonTypeInfo>> Modifiers { get; }

    public AbpSystemTextJsonSerializerModifiersOptions()
    {
        Modifiers = new List<Action<JsonTypeInfo>>
        {
            AbpIncludeExtraPropertiesModifiers.Modify,
        };
    }
}
