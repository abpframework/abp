using System;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.OperationRateLimiting;

[CacheName("OperationRateLimiting")]
[IgnoreMultiTenancy]
public class OperationRateLimitingCacheItem
{
    public int Count { get; set; }

    public DateTimeOffset WindowStart { get; set; }
}
