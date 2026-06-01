using System;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpPasswordResetTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public AbpPasswordResetTokenProviderOptions()
    {
        Name = AbpPasswordResetTokenProvider.ProviderName;
        TokenLifespan = TimeSpan.FromHours(2);
    }
}
