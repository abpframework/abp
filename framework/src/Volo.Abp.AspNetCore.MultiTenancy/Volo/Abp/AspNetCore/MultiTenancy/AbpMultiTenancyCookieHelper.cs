using System;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public static class AbpMultiTenancyCookieHelper
    {
        public static void SetTenantCookie(
            HttpContext context,
            Guid? tenantId,
            string tenantKey)
        {
            if (tenantId != null)
            {
                context.Response.Cookies.Append(
                    tenantKey,
                    tenantId.ToString(),
                    new CookieOptions
                    {
                        Path = "/",
                        HttpOnly = false,
                        Expires = DateTimeOffset.Now.AddYears(10),
                        IsEssential = true
                    }
                );
            }
            else
            {
                context.Response.Cookies.Delete(tenantKey);
            }
        }
    }
}
