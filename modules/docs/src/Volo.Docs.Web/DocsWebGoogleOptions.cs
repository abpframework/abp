using System;
using System.Collections.Generic;
using System.Globalization;

namespace Volo.Docs;

public class DocsWebGoogleOptions
{
    public bool EnableGoogleTranslate { get; set; }

    /// <summary>
    /// https://cloud.google.com/translate/docs/languages
    /// </summary>
    public List<string> IncludedLanguages { get; set; }

    public Func<CultureInfo, string> GetCultureLanguageCode { get; set; }

    public bool EnableGoogleProgrammableSearchEngine { get; set; }

    public string GoogleSearchEngineId { get; set; }

    public DocsWebGoogleOptions()
    {
        EnableGoogleTranslate = false;
        IncludedLanguages =
        [
            "en",
            "tr",
            "zh-CN",
            "zh-TW",
            "ar",
            "cs",
            "hu",
            "hr",
            "fi",
            "fr",
            "hi",
            "it",
            "pt",
            "ru",
            "sk",
            "de",
            "es"
        ];
        GetCultureLanguageCode = culture =>
        {
            return culture.Name switch
            {
                "zh-Hans" => "zh-CN",
                "zh-Hant" => "zh-TW",
                _ => culture.TwoLetterISOLanguageName
            };
        };

        EnableGoogleProgrammableSearchEngine = false;
    }
}
