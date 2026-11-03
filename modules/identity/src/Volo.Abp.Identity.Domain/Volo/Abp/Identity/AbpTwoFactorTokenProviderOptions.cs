using System;

namespace Volo.Abp.Identity;

public abstract class AbpTwoFactorTokenProviderOptions
{
    /// <summary>Default: 3 minutes.</summary>
    public TimeSpan TokenLifespan { get; set; } = TimeSpan.FromMinutes(3);

    /// <summary>Default: 6. Valid range: 1 to 9 (inclusive).</summary>
    public int CodeLength { get; set; } = 6;
}
