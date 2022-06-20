using System.Collections.Generic;
using System.Dynamic;

namespace Volo.CmsKit.Web.Contents;

public static class DictionaryDynamicExtensions
{
    public static dynamic ConvertToDynamicObject(this Dictionary<string, object> dict) //TODO: Move to AbpDictionaryExtensions
    {
        var expandoObject = new ExpandoObject();
        var eoColl = (ICollection<KeyValuePair<string, object>>)expandoObject;

        foreach (var kvp in dict)
        {
            eoColl.Add(kvp);
        }

        return expandoObject;
    }
}

