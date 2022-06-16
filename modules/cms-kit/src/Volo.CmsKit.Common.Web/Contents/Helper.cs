using System.Collections.Generic;
using System.Dynamic;

namespace Volo.CmsKit.Web.Contents;

public static class Helper
{
    public static dynamic ConvertToDynamicObject(this Dictionary<string, object> dict)
    {
        var eo = new ExpandoObject();
        var eoColl = (ICollection<KeyValuePair<string, object>>)eo;

        foreach (var kvp in dict)
        {
            eoColl.Add(kvp);
        }

        dynamic eoDynamic = eo;

        return eoDynamic;
    }
}

