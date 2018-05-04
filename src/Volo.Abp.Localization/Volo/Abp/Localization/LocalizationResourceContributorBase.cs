using System;
using System.Collections.Generic;

namespace Volo.Abp.Localization
{
    //TODO: Delete this class!
    public abstract class LocalizationResourceContributorBase : ILocalizationResourceContributor
    {
        public event EventHandler Updated;

        public abstract List<ILocalizationDictionary> GetDictionaries(LocalizationResourceInitializationContext context);
        
        protected virtual void OnUpdated()
        {
            Updated.InvokeSafely(this);
        }
    }
}