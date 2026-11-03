using System;

namespace Volo.Abp.Identity;

public class AbpPasswordResetTokenProviderOptions : AbpDataProtectionTokenProviderOptions
{
    public AbpPasswordResetTokenProviderOptions()
    {
        Name = AbpPasswordResetTokenProvider.ProviderName;
        TokenLifespan = TimeSpan.FromHours(2);
    }
}
