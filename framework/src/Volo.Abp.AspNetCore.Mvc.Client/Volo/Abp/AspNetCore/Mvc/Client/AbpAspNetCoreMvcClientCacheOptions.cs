using System;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class AbpAspNetCoreMvcClientCacheOptions
{
    public TimeSpan TenantConfigurationCacheAbsoluteExpiration { get; set; }

    public TimeSpan ApplicationConfigurationDtoCacheAbsoluteExpiration { get; set; }

    public AbpAspNetCoreMvcClientCacheOptions()
    {
        TenantConfigurationCacheAbsoluteExpiration = TimeSpan.FromMinutes(5);
        ApplicationConfigurationDtoCacheAbsoluteExpiration = TimeSpan.FromSeconds(300);
    }
}
