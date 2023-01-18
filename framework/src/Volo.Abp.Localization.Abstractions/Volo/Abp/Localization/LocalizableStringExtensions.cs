using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public static class LocalizableStringExtensions
{
    public static async Task<LocalizedString> LocalizeAsync(
        this ILocalizableString localizableString,
        IStringLocalizerFactory stringLocalizerFactory)
    {
        if (localizableString is IAsyncLocalizableString asyncLocalizableString)
        {
            return await asyncLocalizableString.LocalizeAsync(stringLocalizerFactory);
        }

        return localizableString.Localize(stringLocalizerFactory);
    }
}