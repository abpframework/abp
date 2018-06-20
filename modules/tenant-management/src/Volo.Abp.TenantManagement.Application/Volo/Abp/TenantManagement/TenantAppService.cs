using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.TenantManagement
{
    [Authorize(TenantManagementPermissions.Tenants.Default)]
    public class TenantAppService : TenantManagementAppServiceBase, ITenantAppService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantManager _tenantManager;

        public TenantAppService(ITenantRepository tenantRepository, ITenantManager tenantManager)
        {
            _tenantRepository = tenantRepository;
            _tenantManager = tenantManager;
        }

        public async Task<TenantDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Tenant, TenantDto>(
                await _tenantRepository.GetAsync(id)
            );
        }

        public async Task<PagedResultDto<TenantDto>> GetListAsync(GetTenantsInput input)
        {
            var count = await _tenantRepository.GetCountAsync();
            var list = await _tenantRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

            return new PagedResultDto<TenantDto>(
                count,
                ObjectMapper.Map<List<Tenant>, List<TenantDto>>(list)
            );
        }

        [Authorize(TenantManagementPermissions.Tenants.Create)]
        public async Task<TenantDto> CreateAsync(TenantCreateDto input)
        {
            var tenant = await _tenantManager.CreateAsync(input.Name);
            await _tenantRepository.InsertAsync(tenant);
            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }

        [Authorize(TenantManagementPermissions.Tenants.Update)]
        public async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
        {
            var tenant = await _tenantRepository.GetAsync(id);
            await _tenantManager.ChangeNameAsync(tenant, input.Name);
            await _tenantRepository.UpdateAsync(tenant);
            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }

        [Authorize(TenantManagementPermissions.Tenants.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var tenant = await _tenantRepository.FindAsync(id);
            if (tenant == null)
            {
                return;
            }

            await _tenantRepository.DeleteAsync(tenant);
        }
    }
}
