using System;

namespace Volo.Abp.Identity;

public class AbpEmailConfirmationTokenProviderOptions : AbpDataProtectionTokenProviderOptions
{
    public AbpEmailConfirmationTokenProviderOptions()
    {
        Name = AbpEmailConfirmationTokenProvider.ProviderName;
        TokenLifespan = TimeSpan.FromHours(2);
    }
}
