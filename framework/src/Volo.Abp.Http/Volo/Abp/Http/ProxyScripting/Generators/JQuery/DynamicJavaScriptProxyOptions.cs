using System.Collections.Generic;

namespace Volo.Abp.Http.ProxyScripting.Generators.JQuery
{
    public class DynamicJavaScriptProxyOptions
    {
        public HashSet<string> DisabledModules { get; }

        public DynamicJavaScriptProxyOptions()
        {
            DisabledModules = new HashSet<string>();
        }

        public void DisableModule(string module)
        {
            DisabledModules.AddIfNotContains(module);
        }

        public void EnableModule(string module)
        {
            DisabledModules.Remove(module);
        }
    }
}
