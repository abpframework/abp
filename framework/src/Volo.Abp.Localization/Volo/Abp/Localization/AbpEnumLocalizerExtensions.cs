using System;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public static class AbpEnumLocalizerExtensions
{
    public static string GetString<TEnum>(this IAbpEnumLocalizer abpEnumLocalizer, object enumValue)
        where TEnum : Enum
    {
        return abpEnumLocalizer.GetString(typeof(TEnum), enumValue);
    }

    public static string GetString<TEnum>(this IAbpEnumLocalizer abpEnumLocalizer, object enumValue, IStringLocalizer[] specifyLocalizers)
        where TEnum : Enum
    {
        return abpEnumLocalizer.GetString(typeof(TEnum), enumValue, specifyLocalizers);
    }
}
