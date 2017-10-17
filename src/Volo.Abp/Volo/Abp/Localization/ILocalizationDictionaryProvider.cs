using System.Collections.Generic;

namespace Volo.Abp.Localization
{
    public interface ILocalizationDictionaryProvider
    {
        IDictionary<string, ILocalizationDictionary> Dictionaries { get; }

        void Initialize(LocalizationResource resource);
        
        void Extend(ILocalizationDictionary dictionary);
    }
}