using System;

namespace Volo.Abp.DistributedLocking.Dapr;

public class AbpDistributedLockDaprOptions
{
    public string StoreName { get; set; }

    public string? Owner { get; set; }

    public TimeSpan DefaultExpirationTimeout { get; set; }

    public AbpDistributedLockDaprOptions()
    {
        DefaultExpirationTimeout = TimeSpan.FromMinutes(2);
    }
}
