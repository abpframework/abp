using System;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using Volo.Abp.Data;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Json.SystemTextJson;

public class AbpSystemTextJsonSerializerModifiersOptions
{
    public List<Action<JsonTypeInfo>> Modifiers { get; }

    public AbpSystemTextJsonSerializerModifiersOptions()
    {
        Modifiers = new List<Action<JsonTypeInfo>>
        {
            new IncludeNonPublicPropertiesModifiers<ExtensibleObject, ExtraPropertyDictionary>().CreateModifyAction("ExtraProperties")
        };
    }
}
