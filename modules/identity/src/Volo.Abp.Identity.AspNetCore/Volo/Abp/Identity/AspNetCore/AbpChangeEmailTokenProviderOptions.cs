using System;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpChangeEmailTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public AbpChangeEmailTokenProviderOptions()
    {
        Name = AbpChangeEmailTokenProvider.ProviderName;
        TokenLifespan = TimeSpan.FromHours(2);
    }
}
