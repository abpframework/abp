using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Volo.Abp.Localization
{
    public class CombinedLocalizationDictionary : ILocalizationDictionary
    {
        public string CultureName { get; }

        private readonly List<ILocalizationDictionary> _dictionaries;

        public CombinedLocalizationDictionary(params ILocalizationDictionary[] dictionaries)
        {
            Check.NotNullOrEmpty(dictionaries, nameof(dictionaries));
            _dictionaries = dictionaries.ToList();
            CultureName = dictionaries.First().CultureName;

            if (dictionaries.Any(d => d.CultureName != CultureName))
            {
                throw new AbpException($"All given dictionaries should have the same {nameof(CultureName)}");
            }
        }

        public LocalString GetOrNull(string name)
        {
            foreach (var dictionary in _dictionaries)
            {
                var value = dictionary.GetOrNull(name);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        public IReadOnlyList<LocalString> GetAllStrings()
        {
            var localStrings = new Dictionary<string, LocalString>();

            foreach (var dictionary in _dictionaries)
            {
                foreach (var localString in dictionary.GetAllStrings())
                {
                    localStrings[localString.Name] = localString;
                }
            }

            return localStrings.Values.ToImmutableList();
        }

        public void AddFirst(ILocalizationDictionary dictionary)
        {
            _dictionaries.AddFirst(dictionary);
        }

        public void AddLast(ILocalizationDictionary dictionary)
        {
            _dictionaries.AddLast(dictionary);
        }
    }
}
