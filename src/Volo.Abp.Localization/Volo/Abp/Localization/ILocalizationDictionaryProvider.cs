using System;
using System.Collections.Generic;

namespace Volo.Abp.Localization
{
    public interface ILocalizationDictionaryProvider
    {
        IDictionary<string, ILocalizationDictionary> Dictionaries { get; }

        event EventHandler Updated;

        void Initialize(LocalizationResourceInitializationContext context);

        void Extend(ILocalizationDictionaryProvider dictionaryProvider);
    }
}