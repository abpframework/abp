using System;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpChangeEmailTokenProviderOptions : AbpDataProtectionTokenProviderOptions
{
    public AbpChangeEmailTokenProviderOptions()
    {
        Name = AbpChangeEmailTokenProvider.ProviderName;
        TokenLifespan = TimeSpan.FromHours(2);
    }
}
