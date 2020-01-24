using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Volo.Abp.TenantManagement
{
    [Authorize(TenantManagementPermissions.Tenants.Default)]
    public class TenantAppService : TenantManagementAppServiceBase, ITenantAppService
    {
        protected IDataSeeder DataSeeder { get; }
        protected ITenantRepository TenantRepository { get; }
        protected ITenantManager TenantManager { get; }

        public TenantAppService(
            ITenantRepository tenantRepository, 
            ITenantManager tenantManager,
            IDataSeeder dataSeeder)
        {
            DataSeeder = dataSeeder;
            TenantRepository = tenantRepository;
            TenantManager = tenantManager;
        }

        public virtual async Task<TenantDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Tenant, TenantDto>(
                await TenantRepository.GetAsync(id).ConfigureAwait(false));
        }

        public virtual async Task<PagedResultDto<TenantDto>> GetListAsync(GetTenantsInput input)
        {
            var count = await TenantRepository.GetCountAsync(input.Filter).ConfigureAwait(false);
            var list = await TenantRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter).ConfigureAwait(false);

            return new PagedResultDto<TenantDto>(
                count,
                ObjectMapper.Map<List<Tenant>, List<TenantDto>>(list)
            );
        }

        [Authorize(TenantManagementPermissions.Tenants.Create)]
        public virtual async Task<TenantDto> CreateAsync(TenantCreateDto input)
        {
            var tenant = await TenantManager.CreateAsync(input.Name).ConfigureAwait(false);
            await TenantRepository.InsertAsync(tenant).ConfigureAwait(false);

            using (CurrentTenant.Change(tenant.Id, tenant.Name))
            {
                //TODO: Handle database creation?

                //TODO: Set admin email & password..?
                await DataSeeder.SeedAsync(tenant.Id).ConfigureAwait(false);
            }
            
            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }

        [Authorize(TenantManagementPermissions.Tenants.Update)]
        public virtual async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
        {
            var tenant = await TenantRepository.GetAsync(id).ConfigureAwait(false);
            await TenantManager.ChangeNameAsync(tenant, input.Name).ConfigureAwait(false);
            await TenantRepository.UpdateAsync(tenant).ConfigureAwait(false);
            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }

        [Authorize(TenantManagementPermissions.Tenants.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var tenant = await TenantRepository.FindAsync(id).ConfigureAwait(false);
            if (tenant == null)
            {
                return;
            }

            await TenantRepository.DeleteAsync(tenant).ConfigureAwait(false);
        }

        [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task<string> GetDefaultConnectionStringAsync(Guid id)
        {
            var tenant = await TenantRepository.GetAsync(id).ConfigureAwait(false);
            return tenant?.FindDefaultConnectionString();
        }

        [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString)
        {
            var tenant = await TenantRepository.GetAsync(id).ConfigureAwait(false);
            tenant.SetDefaultConnectionString(defaultConnectionString);
            await TenantRepository.UpdateAsync(tenant).ConfigureAwait(false);
        }

        [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task DeleteDefaultConnectionStringAsync(Guid id)
        {
            var tenant = await TenantRepository.GetAsync(id).ConfigureAwait(false);
            tenant.RemoveDefaultConnectionString();
            await TenantRepository.UpdateAsync(tenant).ConfigureAwait(false);
        }
    }
}
