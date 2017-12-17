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

        /// <inheritdoc/>
        public virtual LocalString this[string name]
        {
            get => GetOrNull(name);
            set => _dictionary[name] = value;
        }

        private readonly Dictionary<string, LocalString> _dictionary;

        /// <summary>
        /// Creates a new <see cref="LocalizationDictionary"/> object.
        /// </summary>
        /// <param name="cultureName">Culture of the dictionary</param>
        public LocalizationDictionary(string cultureName)
        {
            CultureName = cultureName;
            _dictionary = new Dictionary<string, LocalString>();
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