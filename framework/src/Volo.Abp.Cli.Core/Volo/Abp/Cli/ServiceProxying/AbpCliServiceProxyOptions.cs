using System;
using System.Collections.Generic;

namespace Volo.Abp.Cli.ServiceProxying
{
    public class AbpCliServiceProxyOptions
    {
        public IDictionary<string, Type> Generators { get; }

        public AbpCliServiceProxyOptions()
        {
            Generators = new Dictionary<string, Type>();
        }
    }
}
