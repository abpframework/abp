using System;

namespace Volo.Abp.Identity;

public class AbpChangeEmailTokenProviderOptions : AbpDataProtectionTokenProviderOptions
{
    public AbpChangeEmailTokenProviderOptions()
    {
        Name = AbpChangeEmailTokenProvider.ProviderName;
        TokenLifespan = TimeSpan.FromHours(2);
    }
}
