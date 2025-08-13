using System;
using System.Collections.Generic;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Mapperly;

// [AttributeUsage(AttributeTargets.Class)]
// public class MapExtraPropertiesAttribute : Attribute
// {
//     public MappingPropertyDefinitionChecks DefinitionChecks { get; set; } = MappingPropertyDefinitionChecks.Null;
//
//     public string[]? IgnoredProperties { get; set; }
//
//     public bool MapToRegularProperties { get; set; }
// }


[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class BeforeMap<TSource> : Attribute
{
    public abstract void Execute(TSource source);
    
    public Type GetSourceType()
    {
        return typeof(TSource);
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class AfterMap<TSource, TDestination> : Attribute
{
    public abstract void Execute(TSource source, TDestination destination);
    
    public Type GetSourceType()
    {
        return typeof(TSource);
    }
    
    public Type GetDestinationType()
    {
        return typeof(TDestination);
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class AfterMap<TSource, TDestination, TContext> : Attribute
{
    public abstract TContext CreateContext(TSource source);
    public abstract TContext CreateContextWithDestination(TSource source, TDestination destination);
    public abstract void Execute(TSource source, TDestination destination, TContext context);
}

[AttributeUsage(AttributeTargets.Class)]
public class MapExtraPropertiesAttribute : AfterMap<IHasExtraProperties, IHasExtraProperties, ExtraPropertyDictionary>
{
    public MappingPropertyDefinitionChecks DefinitionChecks { get; set; } = MappingPropertyDefinitionChecks.Null;

    public string[]? IgnoredProperties { get; set; }

    public bool MapToRegularProperties { get; set; }
    
    protected virtual void MapExtraProperties<TSource, TDestination>(
        IHasExtraProperties source,
        IHasExtraProperties destination,
        ExtraPropertyDictionary destinationExtraProperty,
        MappingPropertyDefinitionChecks? definitionChecks = null,
        string[]? ignoredProperties = null,
        bool mapToRegularProperties = false)
    {
        var result = destinationExtraProperty.IsNullOrEmpty()
            ? new Dictionary<string, object?>()
            : new Dictionary<string, object?>(destinationExtraProperty);

        if (source.ExtraProperties != null && destination.ExtraProperties != null)
        {
            ExtensibleObjectMapper
                .MapExtraPropertiesTo(
                    typeof(TSource),
                    typeof(TDestination),
                    source.ExtraProperties,
                    result,
                    definitionChecks,
                    ignoredProperties
                );
        }

        ObjectHelper.TrySetProperty(destination, x => x.ExtraProperties, () => new ExtraPropertyDictionary(result));
        if (mapToRegularProperties)
        {
            destination.SetExtraPropertiesToRegularProperties();
        }
    }
    
    protected virtual ExtraPropertyDictionary GetExtraProperties<TDestination>(TDestination destination)
    {
        var extraProperties = new ExtraPropertyDictionary();
        if (destination is not IHasExtraProperties hasExtraProperties)
        {
            return extraProperties;
        }

        foreach (var property in hasExtraProperties.ExtraProperties)
        {
            extraProperties.Add(property.Key, property.Value);
        }
        return extraProperties;
    }

    public override ExtraPropertyDictionary CreateContext(IHasExtraProperties source)
    {
        return new ExtraPropertyDictionary();
    }

    public override ExtraPropertyDictionary CreateContextWithDestination(IHasExtraProperties source, IHasExtraProperties destination)
    {
        return GetExtraProperties(destination);
    }

    public override void Execute(IHasExtraProperties source, IHasExtraProperties destination, ExtraPropertyDictionary context)
    {
        MapExtraProperties<IHasExtraProperties, IHasExtraProperties>(
            source,
            destination,
            context,
            DefinitionChecks,
            IgnoredProperties,
            MapToRegularProperties
        );
    }
}