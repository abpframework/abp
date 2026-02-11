using System;

namespace Volo.Abp.Security.Claims;

[Serializable]
public class AbpDynamicClaim
{
    public string Type { get; set; }

    public string? Value { get; set; }

    public AbpDynamicClaim(string type, string? value)
    {
        Type = type;
        Value = value;
    }
}
