using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Volo.Abp.Localization
{
    /// <summary>
    /// Represents a simple implementation of <see cref="ILocalizationDictionary"/> interface.
    /// </summary>
    public class LocalizationDictionary : ILocalizationDictionary, IEnumerable<LocalString>
    {
        /// <inheritdoc/>
        public string CultureName { get; }

        private readonly Dictionary<string, LocalString> _dictionary;

        /// <summary>
        /// Creates a new <see cref="LocalizationDictionary"/> object.
        /// </summary>
        /// <param name="cultureName">Culture of the dictionary</param>
        /// <param name="dictionary">The dictionary</param>
        public LocalizationDictionary(string cultureName, Dictionary<string, LocalString> dictionary)
        {
            CultureName = cultureName;
            _dictionary = dictionary;
        }

        /// <inheritdoc/>
        public virtual LocalString GetOrNull(string name)
        {
            return _dictionary.GetOrDefault(name);
        }

        /// <inheritdoc/>
        public virtual IReadOnlyList<LocalString> GetAllStrings()
        {
            return _dictionary.Values.ToImmutableList();
        }

        public void Extend(ILocalizationDictionary dictionary)
        {
            foreach (var localizedString in dictionary.GetAllStrings())
            {
                _dictionary[localizedString.Name] = localizedString;
            }
        }

        /// <inheritdoc/>
        public virtual IEnumerator<LocalString> GetEnumerator()
        {
            return GetAllStrings().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAllStrings().GetEnumerator();
        }

        protected bool Contains(string name)
        {
            return _dictionary.ContainsKey(name);
        }
    }
}