using System;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity;

public class AbpDefaultTokenProviderOptions : AbpDataProtectionTokenProviderOptions
{
    public AbpDefaultTokenProviderOptions()
    {
        Name = TokenOptions.DefaultProvider;
        TokenLifespan = TimeSpan.FromMinutes(10);
    }
}
