using System;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpEmailLoginTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public AbpEmailLoginTokenProviderOptions()
    {
        Name = AbpEmailLoginTokenProvider.ProviderName;
        TokenLifespan = TimeSpan.FromSeconds(90);
    }
}
