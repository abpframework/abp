using System;
using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpLinkUserTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public AbpLinkUserTokenProviderOptions()
    {
        Name = LinkUserTokenProviderConsts.LinkUserTokenProviderName;
        TokenLifespan = TimeSpan.FromMinutes(10);
    }
}
