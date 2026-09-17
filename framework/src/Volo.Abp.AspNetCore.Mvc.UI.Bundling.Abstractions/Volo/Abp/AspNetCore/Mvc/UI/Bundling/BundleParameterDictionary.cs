using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class BundleParameterDictionary : Dictionary<string, string>
{
    public const string InteractiveAutoPropertyName = "InteractiveAuto";

    public bool InteractiveAuto
    {
        get
        {
            return TryGetValue(InteractiveAutoPropertyName, out var value) && bool.Parse(value);
        }
        set
        {
            this[InteractiveAutoPropertyName] = value.ToString();
        }
    }
}
