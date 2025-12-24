using System;
using System.Collections.Generic;

namespace Volo.Abp.LowCode;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ReferenceEntityAttribute : Attribute
{
    public string DefaultDisplayProperty { get; set; }
    
    public string[] DisplayProperties { get; set; }
    
    public ReferenceEntityAttribute(string defaultDisplayProperty, params string[] displayProperties)
    {
        DefaultDisplayProperty = defaultDisplayProperty;
        DisplayProperties = displayProperties;
    }
    
    public static bool IsReferenceEntity(Type type)
    {
        Check.NotNull(type, nameof(type));
        
        if (type is not { IsAbstract: false, IsInterface: false })
        {
            return false;
        }
        
        return IsDefined(type, typeof(ReferenceEntityAttribute));
    }
    
    public static string GetEntityName(Type type)
    {
        return type.FullName!;
    }
    
    public static string GetDefaultDisplayProperty(Type type)
    {
        var attribute = (ReferenceEntityAttribute?)GetCustomAttribute(type, typeof(ReferenceEntityAttribute));
        if (attribute == null)
        {
            throw new InvalidOperationException($"The type '{type.FullName}' is not marked with ReferenceEntityAttribute.");
        }

        return attribute.DefaultDisplayProperty;
    }
    
    public static string[] GetDisplayProperties(Type type)
    {
        var attribute = (ReferenceEntityAttribute?)GetCustomAttribute(type, typeof(ReferenceEntityAttribute));
        if (attribute == null)
        {
            throw new InvalidOperationException($"The type '{type.FullName}' is not marked with ReferenceEntityAttribute.");
        }

        var displayProperties = attribute.DisplayProperties;
        if (displayProperties.Contains(attribute.DefaultDisplayProperty))
        {
            return displayProperties;
        }

        var list = new List<string>(displayProperties)
        {
            attribute.DefaultDisplayProperty
        };
        return list.ToArray();
    }
}