using System;
using System.Collections.Generic;

namespace Volo.Abp.Localization
{
    public interface ILocalizationResourceContributor
    {
        event EventHandler Updated;

        List<ILocalizationDictionary> GetDictionaries(LocalizationResourceInitializationContext context);
    }
}