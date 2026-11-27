using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.AI;

namespace Volo.Abp.AgentTools;

/// <summary>
/// The JSON schema of a tool's parameters exactly as the model gets it (<c>AgentToolFunction.JsonSchema</c>).
/// </summary>
public static class AgentToolJsonSchema
{
    // Schemas depend on the definition only; definitions live as long as their catalogue.
    private static readonly ConditionalWeakTable<AgentToolDefinition, object> Schemas = new();

    private static readonly string[] NumericKeywords = ["minimum", "maximum", "exclusiveMinimum", "exclusiveMaximum"];

    private static readonly HashSet<string> PagingInterfaces =
    [
        "Volo.Abp.Application.Dtos.ILimitedResultRequest",
        "Volo.Abp.Application.Dtos.IPagedResultRequest",
        "Volo.Abp.Application.Dtos.ISortedResultRequest"
    ];

    public static JsonElement Get(AgentToolDefinition definition) => (JsonElement)Schemas.GetValue(definition, d => Create(d));

    /// <summary>
    /// Parameters of the interface method (MEAI leaves out the CancellationToken), with descriptions and required-ness from
    /// the definition. Built with MEAI's default options, not ABP's: ABP's custom converters (Guid, bool, enum) make the
    /// schema exporter emit empty schemas. Both use web defaults (camelCase), so the shape matches what ABP's options bind;
    /// MEAI's options write enums as their names, as the invoker reads and writes them.
    /// MEAI maps the DataAnnotations ([StringLength], [MinLength]/[MaxLength], [Range], [RegularExpression], [EmailAddress], ...)
    /// and the C# <c>required</c> modifier; <see cref="Transform"/> adds the other required properties and fixes [Range] bounds.
    /// </summary>
    private static JsonElement Create(AgentToolDefinition definition)
    {
        var descriptions = definition.Parameters.ToDictionary(p => p.Name, p => p.Description);
        var schema = AIJsonUtilities.CreateFunctionJsonSchema(
            definition.InterfaceMethod,
            serializerOptions: AIJsonUtilities.DefaultOptions,
            inferenceOptions: new AIJsonSchemaCreateOptions
            {
                ParameterDescriptionProvider = p => descriptions.GetValueOrDefault(p.Name!),
                TransformSchemaNode = (context, node) => Transform(context, node, definition.RequireNonNullableValueTypes)
            });

        var node = JsonNode.Parse(schema.GetRawText())!.AsObject();
        node.Remove("required");
        var required = definition.Parameters.Where(p => p.IsRequired).Select(p => (JsonNode?)p.Name).ToArray();
        if (required.Length > 0)
        {
            node["required"] = new JsonArray(required);
        }

        return JsonSerializer.Deserialize<JsonElement>(node.ToJsonString());
    }

    /// <summary>Runs on every schema node (nested objects and collection items too), after MEAI's own DataAnnotations mapping.</summary>
    private static JsonNode Transform(AIJsonSchemaCreateContext context, JsonNode node, bool requireNonNullableValueTypes)
    {
        if (node is not JsonObject schema)
        {
            return node;
        }

        foreach (var keyword in NumericKeywords)
        {
            FixNumber(schema, keyword);
        }

        // A repeated type is a "$ref" node without properties; its target got the list.
        if (context.TypeInfo.Kind == JsonTypeInfoKind.Object && schema["properties"] is JsonObject properties)
        {
            // The exporter's list (C# required, [JsonRequired], constructor parameters without a default) is kept, except
            // for ABP's paging and sorting members, which are optional unless explicitly required.
            var listed = (schema["required"] as JsonArray)?.Select(n => n?.GetValue<string>()).ToHashSet() ?? [];
            var required = context.TypeInfo.Properties
                .Where(p => properties.ContainsKey(p.Name)
                            && (IsExplicitlyRequired(p)
                                || (!IsPagingMember(context.TypeInfo.Type, p)
                                    && (listed.Contains(p.Name) || IsRequired(context.TypeInfo, p, requireNonNullableValueTypes)))))
                .Select(p => (JsonNode?)p.Name)
                .ToArray();
            schema.Remove("required");
            if (required.Length > 0)
            {
                schema["required"] = new JsonArray(required);
            }
        }

        return schema;
    }

    /// <summary>
    /// MEAI writes a [Range] bound as the attribute holds it: a string for <c>[Range(typeof(decimal), "0", "...")]</c>
    /// (invalid JSON Schema, rejected by providers) and infinity for <c>double.PositiveInfinity</c> (not writable as JSON).
    /// A finite bound becomes a JSON number; anything else is left out.
    /// </summary>
    private static void FixNumber(JsonObject schema, string keyword)
    {
        if (schema[keyword] is not JsonValue value)
        {
            return;
        }

        if (value.TryGetValue<string>(out var text))
        {
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) && double.IsFinite(parsed))
            {
                schema[keyword] = parsed;
            }
            else
            {
                schema.Remove(keyword);
            }
        }
        else if (value.TryGetValue<double>(out var number) && !double.IsFinite(number))
        {
            schema.Remove(keyword);
        }
    }

    /// <summary>[Required] (inherited from an overridden base property too), [JsonRequired] or the C# <c>required</c> modifier.</summary>
    private static bool IsExplicitlyRequired(JsonPropertyInfo property) =>
        property.IsRequired || HasAttribute(property, typeof(RequiredAttribute));

    /// <summary>
    /// <see cref="MemberInfo.IsDefined"/> ignores inherit on a property; <see cref="Attribute.IsDefined(MemberInfo, Type, bool)"/>
    /// also sees the attribute on an overridden base property, as DataAnnotations validation does.
    /// </summary>
    private static bool HasAttribute(JsonPropertyInfo property, Type attributeType) => property.AttributeProvider switch
    {
        MemberInfo member => Attribute.IsDefined(member, attributeType, inherit: true),
        { } provider => provider.IsDefined(attributeType, true),
        null => false
    };

    /// <summary>
    /// A member of ABP's ILimitedResultRequest / IPagedResultRequest / ISortedResultRequest (SkipCount, MaxResultCount,
    /// Sorting) on a type implementing it: the property's getter implements the interface's (by the interface map, so an
    /// explicit implementation does not make a same-named public property optional). Matched by full name: the interfaces
    /// live in Volo.Abp.Ddd.Application.Contracts, which the domain layer does not reference.
    /// </summary>
    private static bool IsPagingMember(Type type, JsonPropertyInfo property)
    {
        if (type.IsInterface || property.AttributeProvider is not PropertyInfo { GetMethod: { } getter })
        {
            return false;
        }

        var handle = getter.GetBaseDefinition().MethodHandle;
        return type.GetInterfaces()
            .Where(i => i.FullName is { } name && PagingInterfaces.Contains(name))
            .Any(i => type.GetInterfaceMap(i).TargetMethods.Any(m => m.GetBaseDefinition().MethodHandle == handle));
    }

    /// <summary>
    /// When <paramref name="requireNonNullableValueTypes"/>, a settable non-nullable value type without a default: no
    /// initializer (seen on an instance made with the parameterless constructor) and no [DefaultValue].
    /// </summary>
    private static bool IsRequired(JsonTypeInfo typeInfo, JsonPropertyInfo property, bool requireNonNullableValueTypes)
    {
        var type = property.PropertyType;
        if (!requireNonNullableValueTypes || !type.IsValueType || Nullable.GetUnderlyingType(type) != null
            || HasAttribute(property, typeof(DefaultValueAttribute)))
        {
            return false;
        }

        // A property bound to a constructor parameter (a record) is left to the exporter, which lists it when the parameter
        // has no default. Without a parameterless constructor (CreateObject null) no instance shows the default, so any
        // other settable value-type property of the type is treated as optional.
        if (property.AssociatedParameter != null || property.Set == null || property.Get == null || typeInfo.CreateObject == null)
        {
            return false;
        }

        try
        {
            return Equals(property.Get(typeInfo.CreateObject()), Activator.CreateInstance(type));
        }
        catch (Exception)
        {
            return false; // The constructor or getter failed: the default cannot be seen, so nothing is promised.
        }
    }
}
