using System;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpDefaultTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public AbpDefaultTokenProviderOptions()
    {
        Name = TokenOptions.DefaultProvider;
        TokenLifespan = TimeSpan.FromMinutes(10);
    }
}
