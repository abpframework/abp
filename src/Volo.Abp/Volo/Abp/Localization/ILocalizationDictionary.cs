using System.Collections.Generic;

namespace Volo.Abp.Localization
{
    /// <summary>
    /// Represents a dictionary that is used to find a localized string.
    /// </summary>
    public interface ILocalizationDictionary
    {
        string CultureName { get; }

        /// <summary>
        /// Gets/sets a string for this dictionary with given name (key).
        /// </summary>
        /// <param name="name">Name to get/set</param>
        LocalString this[string name] { get; set; }

        /// <summary>
        /// Gets a <see cref="LocalString"/> for given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name (key) to get localized string</param>
        /// <returns>The localized string or null if not found in this dictionary</returns>
        LocalString GetOrNull(string name);

        /// <summary>
        /// Gets a list of all strings in this dictionary.
        /// </summary>
        /// <returns>List of all <see cref="LocalString"/> object</returns>
        IReadOnlyList<LocalString> GetAllStrings();

        void Extend(ILocalizationDictionary dictionary);
    }
}