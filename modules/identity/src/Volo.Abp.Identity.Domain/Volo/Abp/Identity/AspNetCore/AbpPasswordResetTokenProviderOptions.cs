using System;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpPasswordResetTokenProviderOptions : AbpDataProtectionTokenProviderOptions
{
    public AbpPasswordResetTokenProviderOptions()
    {
        Name = AbpPasswordResetTokenProvider.ProviderName;
        TokenLifespan = TimeSpan.FromHours(2);
    }
}
