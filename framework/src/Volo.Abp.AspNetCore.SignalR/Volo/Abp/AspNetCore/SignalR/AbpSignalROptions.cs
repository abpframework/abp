using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.SignalR
{
    public class AbpSignalROptions
    {
        public List<HubConfig> Hubs { get; }

        public AbpSignalROptions()
        {
            Hubs = new List<HubConfig>();
        }
    }
}