using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.MultiTenancy;

namespace Pages.Abp.MultiTenancy
{
    [Route("api/abp/multi-tenancy")]
    public class AbpTenantController : AbpController
    {

        protected ITenantStore TenantStore { get; }

        public AbpTenantController(ITenantStore tenantStore)
        {
            TenantStore = tenantStore;
        }

        [HttpGet]
        [Route("find-tenant/{name}")]
        public async Task<FindTenantResult> FindTenantAsync(string name)
        {
            var tenant = await TenantStore.FindAsync(name);

            if (tenant == null)
            {
                return new FindTenantResult{Success = false};
            }

            return new FindTenantResult
            {
                Success = true,
                TenantId = tenant.Id
            };
        }
    }
}
