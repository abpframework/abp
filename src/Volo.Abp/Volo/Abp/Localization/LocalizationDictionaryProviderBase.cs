using System.Collections.Generic;

namespace Volo.Abp.Localization
{
    public abstract class LocalizationDictionaryProviderBase : ILocalizationDictionaryProvider
    {
        public ILocalizationDictionary DefaultDictionary { get; protected set; }

        public IDictionary<string, ILocalizationDictionary> Dictionaries { get; }

        protected LocalizationDictionaryProviderBase()
        {
            Dictionaries = new Dictionary<string, ILocalizationDictionary>();
        }

        public virtual void Initialize()
        {
        }

        public virtual void Extend(ILocalizationDictionaryProvider dictionaryProvider)
        {
            foreach (var dictionary in dictionaryProvider.Dictionaries.Values)
            {
                Extend(dictionary);
            }
        }

        protected virtual void Extend(ILocalizationDictionary dictionary)
        {
            var existingDictionary = Dictionaries.GetOrDefault(dictionary.CultureName);
            if (existingDictionary == null)
            {
                Dictionaries[dictionary.CultureName] = dictionary;
            }
            else
            {
                Overwrite(existingDictionary, dictionary);
            }
        }

        protected virtual void Overwrite(ILocalizationDictionary existingDictionary, ILocalizationDictionary dictionary)
        {
            foreach (var localizedString in dictionary.GetAllStrings())
            {
                existingDictionary[localizedString.Name] = localizedString;
            }
        }
    }
}