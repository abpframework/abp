using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Volo.Abp.Localization
{
    /// <summary>
    /// Represents a simple implementation of <see cref="ILocalizationDictionary"/> interface.
    /// </summary>
    public class StaticLocalizationDictionary : ILocalizationDictionary
    {
        /// <inheritdoc/>
        public string CultureName { get; }

        protected Dictionary<string, LocalString> Dictionary { get; }

        /// <summary>
        /// Creates a new <see cref="StaticLocalizationDictionary"/> object.
        /// </summary>
        /// <param name="cultureName">Culture of the dictionary</param>
        /// <param name="dictionary">The dictionary</param>
        public StaticLocalizationDictionary(string cultureName, Dictionary<string, LocalString> dictionary)
        {
            CultureName = cultureName;
            Dictionary = dictionary;
        }

        /// <inheritdoc/>
        public virtual LocalString GetOrNull(string name)
        {
            return Dictionary.GetOrDefault(name);
        }

        /// <inheritdoc/>
        public virtual IReadOnlyList<LocalString> GetAllStrings()
        {
            return Dictionary.Values.ToImmutableList();
        }
    }
}