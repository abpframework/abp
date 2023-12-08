using System;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class AbpAspNetCoreMvcClientCacheOptions
{
    public TimeSpan ApplicationConfigurationDtoCacheAbsoluteExpiration { get; set; }

    public AbpAspNetCoreMvcClientCacheOptions()
    {
        ApplicationConfigurationDtoCacheAbsoluteExpiration = TimeSpan.FromSeconds(300);
    }
}
