using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using Volo.Abp.MultiTenancy;

namespace Pages.Abp.MultiTenancy
{
    public class AbpTenantAppService : ApplicationService, IAbpTenantAppService
    {
        protected ITenantStore TenantStore { get; }

        public AbpTenantAppService(ITenantStore tenantStore)
        {
            TenantStore = tenantStore;
        }

        public async Task<FindTenantResult> FindTenantByNameAsync(string name)
        {
            var tenant = await TenantStore.FindAsync(name);

            if (tenant == null)
            {
                return new FindTenantResult { Success = false };
            }

            return new FindTenantResult
            {
                Success = true,
                TenantId = tenant.Id,
                Name = tenant.Name
            };
        }
        
        public async Task<FindTenantResult> FindTenantByIdAsync(Guid id)
        {
            var tenant = await TenantStore.FindAsync(id);

            if (tenant == null)
            {
                return new FindTenantResult { Success = false };
            }

            return new FindTenantResult
            {
                Success = true,
                TenantId = tenant.Id,
                Name = tenant.Name
            };
        }
    }
}