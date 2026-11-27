using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Volo.Abp.AgentTools;

/// <summary>
/// A structural check of a tool input schema against the JSON Schema subset that model providers accept: known
/// <c>type</c>s, <c>required</c> naming existing properties once, finite numeric keywords (lengths and counts
/// non-negative integers), valid <c>pattern</c>s and local <c>$ref</c>s that resolve without a reference-only cycle.
/// Not a JSON Schema meta-validator: unknown keywords are ignored.
/// </summary>
public static class AgentToolSchemaValidator
{
    private static readonly HashSet<string> Types = ["string", "number", "integer", "boolean", "object", "array", "null"];
    private static readonly string[] NumberKeywords = ["minimum", "maximum", "exclusiveMinimum", "exclusiveMaximum", "multipleOf"];
    private static readonly string[] CountKeywords = ["minLength", "maxLength", "minItems", "maxItems", "minProperties", "maxProperties"];
    private static readonly string[] SchemaKeywords = ["additionalProperties", "not", "if", "then", "else", "contains", "propertyNames", "unevaluatedItems", "unevaluatedProperties", "additionalItems"];
    private static readonly string[] SchemaArrayKeywords = ["allOf", "anyOf", "oneOf", "prefixItems"];
    private static readonly string[] SchemaMapKeywords = ["properties", "patternProperties", "$defs", "definitions", "dependentSchemas"];

    /// <summary>The first problem found, as "location: reason" (location a JSON pointer), or null when the schema passes.</summary>
    public static string? Validate(JsonElement schema) => Validate(schema, schema, "#");

    private static string? Validate(JsonElement root, JsonElement node, string path)
    {
        if (node.ValueKind is JsonValueKind.True or JsonValueKind.False)
        {
            return null;
        }

        if (node.ValueKind != JsonValueKind.Object)
        {
            return $"{path}: a schema must be an object or a boolean";
        }

        return CheckType(node, path)
               ?? CheckRequired(node, path)
               ?? CheckNumbers(node, path)
               ?? CheckPatterns(node, path)
               ?? CheckRef(root, node, path)
               ?? CheckSubschemas(root, node, path);
    }

    private static string? CheckType(JsonElement node, string path)
    {
        if (!node.TryGetProperty("type", out var type))
        {
            return null;
        }

        var names = type.ValueKind == JsonValueKind.Array ? type.EnumerateArray().ToList() : [type];
        if (names.Count == 0 || names.Any(n => n.ValueKind != JsonValueKind.String || !Types.Contains(n.GetString()!)))
        {
            return $"{path}: type {type.GetRawText()} is not one of {string.Join(", ", Types)} (or a non-empty array of them)";
        }

        return names.Select(n => n.GetString()).Distinct().Count() == names.Count ? null : $"{path}: type {type.GetRawText()} lists a type twice";
    }

    private static string? CheckRequired(JsonElement node, string path)
    {
        if (!node.TryGetProperty("required", out var required))
        {
            return null;
        }

        if (required.ValueKind != JsonValueKind.Array)
        {
            return $"{path}: required must be an array";
        }

        var hasProperties = node.TryGetProperty("properties", out var properties) && properties.ValueKind == JsonValueKind.Object;
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (var item in required.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.String)
            {
                return $"{path}: required must list property names";
            }

            var name = item.GetString()!;
            if (!names.Add(name))
            {
                return $"{path}: required lists '{name}' twice";
            }

            if (!hasProperties || !properties.TryGetProperty(name, out _))
            {
                return $"{path}: required lists '{name}', which is not in properties";
            }
        }

        return null;
    }

    private static string? CheckNumbers(JsonElement node, string path)
    {
        foreach (var keyword in NumberKeywords)
        {
            if (node.TryGetProperty(keyword, out var value)
                && (value.ValueKind != JsonValueKind.Number || !value.TryGetDouble(out var number) || !double.IsFinite(number)
                    || (keyword == "multipleOf" && number <= 0)))
            {
                return $"{path}: {keyword} {value.GetRawText()} is not a finite JSON number{(keyword == "multipleOf" ? " greater than 0" : "")}";
            }
        }

        foreach (var keyword in CountKeywords)
        {
            if (node.TryGetProperty(keyword, out var value)
                && (value.ValueKind != JsonValueKind.Number || !value.TryGetInt64(out var count) || count < 0))
            {
                return $"{path}: {keyword} {value.GetRawText()} is not a non-negative integer";
            }
        }

        return null;
    }

    /// <summary>
    /// .NET's parser: [RegularExpression] is validated with it on the server, so a pattern it rejects is wrong either way.
    /// Providers use ECMAScript-like engines, which reject some .NET-only syntax this check lets through (inline options
    /// such as <c>(?i)</c>, <c>\A</c>/<c>\Z</c>, named blocks like <c>\p{IsGreek}</c>, character class subtraction).
    /// </summary>
    private static string? CheckPatterns(JsonElement node, string path)
    {
        if (node.TryGetProperty("pattern", out var pattern))
        {
            if (pattern.ValueKind != JsonValueKind.String)
            {
                return $"{path}: pattern must be a string";
            }

            if (GetRegexError(pattern.GetString()!) is { } error)
            {
                return $"{path}: pattern '{pattern.GetString()}' is not a valid regular expression: {error}";
            }
        }

        if (node.TryGetProperty("patternProperties", out var patternProperties) && patternProperties.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in patternProperties.EnumerateObject())
            {
                if (GetRegexError(property.Name) is { } error)
                {
                    return $"{path}/patternProperties: '{property.Name}' is not a valid regular expression: {error}";
                }
            }
        }

        return null;
    }

    private static string? GetRegexError(string pattern)
    {
        try
        {
            _ = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
            return null;
        }
        catch (ArgumentException ex)
        {
            return ex.Message;
        }
    }

    /// <summary>Follows the $ref chain from this node: every reference resolves locally and the chain never comes back on itself.</summary>
    private static string? CheckRef(JsonElement root, JsonElement node, string path)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var current = node;
        while (current.ValueKind == JsonValueKind.Object && current.TryGetProperty("$ref", out var reference))
        {
            if (reference.ValueKind != JsonValueKind.String)
            {
                return $"{path}: $ref must be a string";
            }

            var pointer = reference.GetString()!;
            if (!seen.Add(pointer))
            {
                return $"{path}: $ref cycle through '{pointer}' (references that only point at each other never reach a schema)";
            }

            var target = Resolve(root, pointer);
            if (target is not { ValueKind: JsonValueKind.Object or JsonValueKind.True or JsonValueKind.False })
            {
                return $"{path}: $ref '{pointer}' does not resolve to a schema in this document (only local JSON pointers like '#/properties/x' are supported)";
            }

            current = target.Value;
        }

        return null;
    }

    private static JsonElement? Resolve(JsonElement root, string pointer)
    {
        if (pointer != "#" && !pointer.StartsWith("#/", StringComparison.Ordinal))
        {
            return null;
        }

        var current = root;
        foreach (var segment in pointer == "#" ? [] : pointer[2..].Split('/'))
        {
            var name = Uri.UnescapeDataString(segment).Replace("~1", "/").Replace("~0", "~");
            if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(name, out var next))
            {
                current = next;
            }
            else if (current.ValueKind == JsonValueKind.Array
                     && int.TryParse(name, NumberStyles.None, CultureInfo.InvariantCulture, out var index)
                     && index < current.GetArrayLength())
            {
                current = current[index];
            }
            else
            {
                return null;
            }
        }

        return current;
    }

    private static string? CheckSubschemas(JsonElement root, JsonElement node, string path)
    {
        // items: one schema, or an array of them (draft 4-7 tuple form).
        if (node.TryGetProperty("items", out var items))
        {
            var error = items.ValueKind == JsonValueKind.Array
                ? items.EnumerateArray().Select((item, i) => Validate(root, item, $"{path}/items/{i}")).FirstOrDefault(e => e != null)
                : Validate(root, items, path + "/items");
            if (error != null)
            {
                return error;
            }
        }

        foreach (var keyword in SchemaKeywords)
        {
            if (node.TryGetProperty(keyword, out var value) && Validate(root, value, $"{path}/{keyword}") is { } error)
            {
                return error;
            }
        }

        foreach (var keyword in SchemaArrayKeywords)
        {
            if (!node.TryGetProperty(keyword, out var value))
            {
                continue;
            }

            if (value.ValueKind != JsonValueKind.Array)
            {
                return $"{path}: {keyword} must be an array of schemas";
            }

            var i = 0;
            foreach (var item in value.EnumerateArray())
            {
                if (Validate(root, item, $"{path}/{keyword}/{i++}") is { } error)
                {
                    return error;
                }
            }
        }

        foreach (var keyword in SchemaMapKeywords)
        {
            if (!node.TryGetProperty(keyword, out var value))
            {
                continue;
            }

            if (value.ValueKind != JsonValueKind.Object)
            {
                return $"{path}: {keyword} must be an object of schemas";
            }

            foreach (var property in value.EnumerateObject())
            {
                if (Validate(root, property.Value, $"{path}/{keyword}/{property.Name}") is { } error)
                {
                    return error;
                }
            }
        }

        return null;
    }
}
