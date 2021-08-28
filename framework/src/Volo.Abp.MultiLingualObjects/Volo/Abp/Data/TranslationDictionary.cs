using System;
using System.Collections.Generic;


namespace Volo.Abp.Data
{
    [Serializable]
    public class TranslationDictionary : Dictionary<string, object>
    {
        public TranslationDictionary() : base(StringComparer.OrdinalIgnoreCase)
        {

        }

        public TranslationDictionary(IDictionary<string, object> dictionary)
            : base(dictionary, StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
