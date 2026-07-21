using System;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpEmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public AbpEmailConfirmationTokenProviderOptions()
    {
        Name = AbpEmailConfirmationTokenProvider.ProviderName;
        TokenLifespan = TimeSpan.FromHours(2);
    }
}
