using System.Collections.Generic;

namespace Volo.Docs;

public class DocsWebGoogleTranslationOptions
{
    public bool UseGoogleTranslation { get; set; }

    /// <summary>
    /// https://cloud.google.com/translate/docs/languages
    /// </summary>
    public List<string> IncludedLanguages { get; set; }

    public DocsWebGoogleTranslationOptions()
    {
        UseGoogleTranslation = false;
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
    }
}
