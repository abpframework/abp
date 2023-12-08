using System;

namespace Volo.Abp.AspNetCore.Mvc.MultiTenancy;

public static class TenantConfigurationCacheHelper
{
    public static string CreateCacheKey(string tenantName)
    {
        return $"RemoteTenantStore_Name_{tenantName}";
    }

    public static string CreateCacheKey(Guid tenantId)
    {
        return $"RemoteTenantStore_Id_{tenantId:N}";
    }
}
