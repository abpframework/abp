using System;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpEmailConfirmationTokenProviderOptions : AbpDataProtectionTokenProviderOptions
{
    public AbpEmailConfirmationTokenProviderOptions()
    {
        Name = AbpEmailConfirmationTokenProvider.ProviderName;
        TokenLifespan = TimeSpan.FromHours(2);
    }
}
