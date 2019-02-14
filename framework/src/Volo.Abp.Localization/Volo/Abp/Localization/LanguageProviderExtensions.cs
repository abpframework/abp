using System.Collections.Generic;
using Volo.Abp.Threading;

namespace Volo.Abp.Localization
{
    public static class LanguageProviderExtensions
    {
        public static IReadOnlyList<LanguageInfo> GetLanguages(this ILanguageProvider languageProvider)
        {
            return AsyncHelper.RunSync(languageProvider.GetLanguagesAsync);
        }
    }
}