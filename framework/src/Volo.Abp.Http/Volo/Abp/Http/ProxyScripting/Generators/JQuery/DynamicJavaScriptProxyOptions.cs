using System.Collections.Generic;

namespace Volo.Abp.Http.ProxyScripting.Generators.JQuery
{
    public class DynamicJavaScriptProxyOptions
    {
        public HashSet<string> EnabledModules { get; set; }

        public bool EnabledAllModules { get; set; }

        public DynamicJavaScriptProxyOptions()
        {
            EnabledModules = new HashSet<string> { "app" };
        }
    }
}
