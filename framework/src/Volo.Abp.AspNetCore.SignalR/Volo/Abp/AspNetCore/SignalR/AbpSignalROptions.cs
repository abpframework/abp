using System;

namespace Volo.Abp.AspNetCore.SignalR;

public class AbpSignalROptions
{
    public HubConfigList Hubs { get; }

    /// <summary>
    /// Default: 5 seconds.
    /// </summary>
    public TimeSpan? CheckDynamicClaimsInterval { get; set; }

    public AbpSignalROptions()
    {
        Hubs = new HubConfigList();
        CheckDynamicClaimsInterval = TimeSpan.FromSeconds(5);
    }
}
