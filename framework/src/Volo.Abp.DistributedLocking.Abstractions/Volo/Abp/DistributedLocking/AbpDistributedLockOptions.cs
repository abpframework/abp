namespace Volo.Abp.DistributedLocking;

public class AbpDistributedLockOptions
{
    /// <summary>
    /// DistributedLock key prefix.
    /// </summary>
    public string KeyPrefix  { get; set; }
    
    public AbpDistributedLockOptions()
    {
        KeyPrefix = "";
    }
}