using System;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

/// <summary>
/// Represents a simple implementation of <see cref="ILocalizationDictionary"/> interface.
/// </summary>
public class StaticLocalizationDictionary : ILocalizationDictionary
{
    /// <inheritdoc/>
    public string CultureName { get; }

    protected Dictionary<string, LocalizedString> Dictionary { get; }

    /// <summary>
    /// Creates a new <see cref="StaticLocalizationDictionary"/> object.
    /// </summary>
    /// <param name="cultureName">Culture of the dictionary</param>
    /// <param name="dictionary">The dictionary</param>
    public StaticLocalizationDictionary(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        CultureName = cultureName;
        Dictionary = dictionary;
    }

    /// <inheritdoc/>
    public virtual LocalizedString? GetOrNull(string name)
    {
        return Dictionary.GetOrDefault(name);
    }

    public void Fill(Dictionary<string, LocalizedString> dictionary)
    {
        foreach (var item in Dictionary)
        {
            dictionary[item.Key] = item.Value;
        }
    }

    public void Merge(ILocalizationDictionary dictionary)
    {
        if (dictionary is not StaticLocalizationDictionary staticLocalizationDictionary)
        {
            return; // do nothing, we have no idea what to do with such
        }

        foreach (var item in staticLocalizationDictionary.Dictionary)
        {
            if (string.IsNullOrEmpty(item.Key))
            {
                throw new AbpException("The key is empty in given json string.");
            }

            Dictionary[item.Key] = item.Value;
        }
    }
}
