using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public MultiTenancyController(ITenantStore tenantStore)
        {
            _tenantStore = tenantStore;
        }

        public async Task<ActionResult> SwitchTenant(string tenant = "")
        {
            var tenantInfo = await FindTenantAsync(tenant);
            if (tenantInfo == null)
            {
                throw new UserFriendlyException("Unknown tenant: " + tenant);
            }

            HttpContext.Response.Cookies.Append(
                "__tenant",
                tenantInfo.Id.ToString(),
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(1)
                }
            );

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
