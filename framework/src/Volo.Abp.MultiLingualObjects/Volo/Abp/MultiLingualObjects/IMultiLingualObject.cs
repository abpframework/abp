using System.Collections.Generic;
using Volo.Abp.Data;


namespace Volo.Abp.MultiLingualObjects
{
    public interface IMultiLingualObject
    {
        string DefaultCulture { get; set; }
        
        TranslationDictionary Translations { get; set; }
    }
}
