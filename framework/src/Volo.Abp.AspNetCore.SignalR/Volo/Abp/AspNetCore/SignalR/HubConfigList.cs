using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.AspNetCore.SignalR
{
    public class HubConfigList : List<HubConfig>
    {
        public void AddOrUpdate<THub>(Action<HubConfig> configAction = null)
        {
            AddOrUpdate(typeof(THub));
        }

        public void AddOrUpdate(Type hubType, Action<HubConfig> configAction = null)
        {
            var hubConfig = this.GetOrAdd(
                c => c.HubType == hubType,
                () => HubConfig.Create(hubType)
            );

            configAction?.Invoke(hubConfig);
        }
    }
}