namespace Volo.Abp.AspNetCore.SignalR;

public class AbpSignalROptions
{
    public HubConfigList Hubs { get; }

    public AbpSignalROptions()
    {
        Hubs = new HubConfigList();
    }
}
