using System;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpLinkUserTokenProviderOptions : AbpDataProtectionTokenProviderOptions
{
    public AbpLinkUserTokenProviderOptions()
    {
        Name = LinkUserTokenProviderConsts.LinkUserTokenProviderName;
        TokenLifespan = TimeSpan.FromMinutes(10);
    }
}
