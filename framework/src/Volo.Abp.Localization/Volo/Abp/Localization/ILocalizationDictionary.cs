using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization
{
    /// <summary>
    /// Represents a dictionary that is used to find a localized string.
    /// </summary>
    public interface ILocalizationDictionary
    {
        string CultureName { get; }

        LocalizedString GetOrNull(string name);

        void Fill(Dictionary<string, LocalizedString> dictionary);
    }
}