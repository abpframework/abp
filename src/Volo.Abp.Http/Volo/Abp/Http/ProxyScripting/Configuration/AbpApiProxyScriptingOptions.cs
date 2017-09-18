using System;
using System.Collections.Generic;

namespace Volo.Abp.Http.ProxyScripting.Configuration
{
    public class AbpApiProxyScriptingOptions
    {
        public IDictionary<string, Type> Generators { get; }

        public AbpApiProxyScriptingOptions()
        {
            Generators = new Dictionary<string, Type>();
        }
    }
}