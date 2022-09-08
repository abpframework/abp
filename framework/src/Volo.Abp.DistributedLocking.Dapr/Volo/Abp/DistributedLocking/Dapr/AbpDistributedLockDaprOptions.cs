namespace Volo.Abp.DistributedLocking.Dapr;

public class AbpDistributedLockDaprOptions
{
    public string StoreName { get; set; }
    
    public TimeSpan DefaultTimeout { get; set; }

    public AbpDistributedLockDaprOptions()
    {
        DefaultTimeout = TimeSpan.FromSeconds(30);
    }
}