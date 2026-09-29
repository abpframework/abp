using System;

namespace Volo.Abp.Identity;

/// <summary>
/// Stands in for ASP.NET Core's <c>DataProtectionTokenProviderOptions</c>, which is only available
/// through the ASP.NET Core shared framework.
/// </summary>
public abstract class AbpDataProtectionTokenProviderOptions
{
    /// <summary>
    /// Also the DataProtection purpose, so two providers with different names cannot validate each
    /// other's tokens.
    /// </summary>
    public string Name { get; set; } = "DataProtectorTokenProvider";

    public TimeSpan TokenLifespan { get; set; } = TimeSpan.FromDays(1);
}
