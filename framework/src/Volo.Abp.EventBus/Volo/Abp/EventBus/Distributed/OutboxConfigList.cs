using System;
using System.Collections.Generic;

namespace Volo.Abp.EventBus.Distributed
{
    public class OutboxConfigList : Dictionary<string, OutboxConfig>
    {
        public void Configure(string outboxName, Action<OutboxConfig> configAction)
        {
            var outboxConfig = this.GetOrAdd(outboxName, () => new OutboxConfig(outboxName));
            configAction(outboxConfig);
        }
    }
}