using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization.Json;

public static class JsonLocalizationDictionaryBuilder
{
    /// <summary>
    ///     Builds an <see cref="JsonLocalizationDictionaryBuilder" /> from given file.
    /// </summary>
    /// <param name="filePath">Path of the file</param>
    public static ILocalizationDictionary? BuildFromFile(string filePath)
    {
        try
        {
            return BuildFromJsonString(File.ReadAllText(filePath));
        }
        catch (Exception ex)
        {
            throw new AbpException("Invalid localization file format: " + filePath, ex);
        }
    }

    private static readonly JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    /// <summary>
    ///     Builds an <see cref="JsonLocalizationDictionaryBuilder" /> from given json string.
    /// </summary>
    /// <param name="jsonString">Json string</param>
    public static ILocalizationDictionary? BuildFromJsonString(string jsonString)
    {
        JsonLocalizationFile? jsonFile;
        try
        {
            jsonFile = JsonSerializer.Deserialize<JsonLocalizationFile>(jsonString, DeserializeOptions);
        }
        catch (JsonException ex)
        {
            throw new AbpException("Can not parse json string. " + ex.Message);
        }
        if (jsonFile == null)
        {
            return null;
        }

        var cultureCode = jsonFile.Culture;
        if (string.IsNullOrEmpty(cultureCode))
        {
            return null;
        }

        var dictionary = new Dictionary<string, LocalizedString>();
        var dublicateNames = new List<string>();

        foreach (var item in FlattenTexts(jsonFile.Texts))
        {
            if (string.IsNullOrEmpty(item.Key))
            {
                throw new AbpException("The key is empty in given json string.");
            }
            if (dictionary.GetOrDefault(item.Key) != null)
            {
                dublicateNames.Add(item.Key);
            }
            dictionary[item.Key] = new LocalizedString(item.Key, item.Value.NormalizeLineEndings());
        }

        if (dublicateNames.Count > 0)
        {
            throw new AbpException(
                "A dictionary can not contain same key twice. There are some duplicated names: " +
                dublicateNames.JoinAsString(", "));
        }

        return new StaticLocalizationDictionary(cultureCode, dictionary);
    }

    private static Dictionary<string, string> FlattenTexts(Dictionary<string, object> texts, string prefix = "")
    {
        var result = new Dictionary<string, string>();
        foreach (var text in texts)
        {
            var currentKey = string.IsNullOrEmpty(prefix) ? text.Key : $"{prefix}__{text.Key}";
            switch (text.Value)
            {
                case JsonElement jsonElement:
                    foreach (var item in FlattenJsonElement(jsonElement, currentKey))
                    {
                        result[item.Key] = item.Value;
                    }
                    break;
                case string str:
                    result[currentKey] = str;
                    break;
                case null:
                    result[currentKey] = "";
                    break;
                default:
                    result[currentKey] = text.Value.ToString() ?? "";
                    break;
            }
        }
        return result;
    }

    private static IEnumerable<KeyValuePair<string, string>> FlattenJsonElement(JsonElement element, string prefix)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                yield return new KeyValuePair<string, string>(prefix, element.GetString() ?? "");
                break;
            case JsonValueKind.Object:
                foreach (var prop in element.EnumerateObject())
                {
                    var newKey = $"{prefix}__{prop.Name}";
                    foreach (var item in FlattenJsonElement(prop.Value, newKey))
                    {
                        yield return item;
                    }
                }
                break;
            case JsonValueKind.Array:
                var i = 0;
                foreach (var prop in element.EnumerateArray())
                {
                    var newKey = $"{prefix}__{i}";
                    foreach (var item in FlattenJsonElement(prop, newKey))
                    {
                        yield return item;
                    }
                    i++;
                }
                break;
            default:
                yield return new KeyValuePair<string, string>(prefix, element.ToString());
                break;
        }
    }
}
