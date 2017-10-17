using System.Collections.Generic;

namespace Volo.Abp.Localization
{
    public abstract class LocalizationDictionaryProviderBase : ILocalizationDictionaryProvider
    {
        public LocalizationResource ResourceType { get; private set; }

        public ILocalizationDictionary DefaultDictionary { get; protected set; }

        public IDictionary<string, ILocalizationDictionary> Dictionaries { get; private set; }

        protected LocalizationDictionaryProviderBase()
        {
            Dictionaries = new Dictionary<string, ILocalizationDictionary>();
        }

        public virtual void Initialize(LocalizationResource resourceType)
        {
            ResourceType = resourceType;
        }

        public void Extend(ILocalizationDictionary dictionary)
        {
            //Add
            ILocalizationDictionary existingDictionary;
            if (!Dictionaries.TryGetValue(dictionary.CultureName, out existingDictionary))
            {
                Dictionaries[dictionary.CultureName] = dictionary;
                return;
            }

            //Override
            var localizedStrings = dictionary.GetAllStrings();
            foreach (var localizedString in localizedStrings)
            {
                existingDictionary[localizedString.Name] = localizedString;
            }
        }
    }
}