using System;

namespace Volo.Abp.Security.Claims;

[Serializable]
public class AbpClaimCacheItem
{
    public string Type { get; set; }

    public string Value { get; set; }

    public AbpClaimCacheItem(string type, string value)
    {
        Type = type;
        Value = value;
    }
}
