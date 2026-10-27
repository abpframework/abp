using System;

namespace Volo.Abp.Identity;

public class AbpLinkUserTokenProviderOptions : AbpDataProtectionTokenProviderOptions
{
    public AbpLinkUserTokenProviderOptions()
    {
        Name = LinkUserTokenProviderConsts.LinkUserTokenProviderName;
        TokenLifespan = TimeSpan.FromMinutes(10);
    }
}
