using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Ui;

namespace AbpDesk.Web.Mvc.Controllers
{
    /* TODO: This is temporary solution to switch tenant.
     */

    public class MultiTenancyController : AbpController
    {
        private readonly ITenantStore _tenantStore;
        private readonly AspNetCoreMultiTenancyOptions _options;

        public MultiTenancyController(ITenantStore tenantStore, IOptions<AspNetCoreMultiTenancyOptions> options)
        {
            _tenantStore = tenantStore;
            _options = options.Value;
        }

        public async Task<ActionResult> SwitchTenant(string tenant = "")
        {
            if (tenant.IsNullOrEmpty())
            {
                HttpContext.Response.Cookies.Delete(_options.TenantKey);
            }
            else
            {
                var tenantInfo = await FindTenantAsync(tenant);
                if (tenantInfo == null)
                {
                    throw new UserFriendlyException("Unknown tenant: " + tenant);
                }

                HttpContext.Response.Cookies.Append(
                    _options.TenantKey,
                    tenantInfo.Id.ToString(),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddYears(1)
                    }
                );
            }

            return Redirect("/");
        }

        private async Task<TenantInfo> FindTenantAsync(string tenantIdOrName)
        {
            if (Guid.TryParse(tenantIdOrName, out var parsedTenantId))
            {
                return await _tenantStore.FindAsync(parsedTenantId);
            }
            else
            {
                return await _tenantStore.FindAsync(tenantIdOrName);
            }
        }
    }
}
