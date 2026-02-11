using System;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Mapperly;

[AttributeUsage(AttributeTargets.Class)]
public class MapExtraPropertiesAttribute : Attribute
{
    public MappingPropertyDefinitionChecks DefinitionChecks { get; set; } = MappingPropertyDefinitionChecks.Null;

    public string[]? IgnoredProperties { get; set; }

    public bool MapToRegularProperties { get; set; }
}
