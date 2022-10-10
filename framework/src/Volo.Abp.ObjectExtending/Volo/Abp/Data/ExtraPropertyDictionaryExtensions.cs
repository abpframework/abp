using System;
using JetBrains.Annotations;

namespace Volo.Abp.Data;

public static class ExtraPropertyDictionaryExtensions
{
    public static T ToEnum<T>(this ExtraPropertyDictionary extraPropertyDictionary, string key)
        where T : Enum
    {
        if (extraPropertyDictionary[key].GetType() == typeof(T))
        {
            return (T)extraPropertyDictionary[key];
        }

        extraPropertyDictionary[key] = Enum.Parse(typeof(T), extraPropertyDictionary[key].ToString(), ignoreCase: true);
        return (T)extraPropertyDictionary[key];
    }

    public static object ToEnum(this ExtraPropertyDictionary extraPropertyDictionary, string key, Type enumType)
    {
        if (!enumType.IsEnum || extraPropertyDictionary[key].GetType() == enumType)
        {
            return extraPropertyDictionary[key];
        }

        extraPropertyDictionary[key] = Enum.Parse(enumType, extraPropertyDictionary[key].ToString(), ignoreCase: true);
        return extraPropertyDictionary[key];
    }

    public static bool HasSameItems(
        [NotNull] this ExtraPropertyDictionary dictionary,
        [NotNull] ExtraPropertyDictionary otherDictionary)
    {
        Check.NotNull(dictionary, nameof(dictionary));
        Check.NotNull(otherDictionary, nameof(otherDictionary));

        if (dictionary.Count != otherDictionary.Count)
        {
            return false;
        }
        
        foreach (var key in dictionary.Keys)
        {
            if (!otherDictionary.ContainsKey(key) || 
                dictionary[key]?.ToString() != otherDictionary[key]?.ToString())
            {
                return false;
            }
        }

        return true;
    }
}