using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using Volo.Abp.MultiTenancy;

namespace Pages.Abp.MultiTenancy
{
    [Authorize(TenantManagementPermissions.Tenants.Default)] //Psuedo as there might be no reference to the 'TenantManagementPermissions' class.
    public class AbpTenantAppService : ApplicationService, IAbpTenantAppService
    {
        protected ITenantStore TenantStore { get; }

        public AbpTenantAppService(ITenantStore tenantStore)
        {
            TenantStore = tenantStore;
        }

        [Authorize(TenantManagementPermissions.Tenants.View)]
        public async Task<FindTenantResultDto> FindTenantByNameAsync(string name)
        {
            var tenant = await TenantStore.FindAsync(name);

            if (tenant == null)
            {
                return new FindTenantResultDto { Success = false };
            }

            return new FindTenantResultDto
            {
                Success = true,
                TenantId = tenant.Id,
                Name = tenant.Name
            };
        }
        

        [Authorize(TenantManagementPermissions.Tenants.View)]
        public async Task<FindTenantResultDto> FindTenantByIdAsync(Guid id)
        {
            var tenant = await TenantStore.FindAsync(id);

            if (tenant == null)
            {
                return new FindTenantResultDto { Success = false };
            }

            return new FindTenantResultDto
            {
                Success = true,
                TenantId = tenant.Id,
                Name = tenant.Name
            };
        }
    }
}
