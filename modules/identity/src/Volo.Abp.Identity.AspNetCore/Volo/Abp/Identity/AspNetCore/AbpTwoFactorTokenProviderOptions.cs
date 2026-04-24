using System;

namespace Volo.Abp.Identity.AspNetCore;

public abstract class AbpTwoFactorTokenProviderOptions
{
    /// <summary>Default: 3 minutes.</summary>
    public TimeSpan TokenLifespan { get; set; } = TimeSpan.FromMinutes(3);

    /// <summary>Default: 6.</summary>
    public int CodeLength { get; set; } = 6;
}
