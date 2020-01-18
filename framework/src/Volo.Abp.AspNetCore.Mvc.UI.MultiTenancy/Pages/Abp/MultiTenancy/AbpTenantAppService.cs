﻿using System;
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

        public async Task<FindTenantResultDto> FindTenantByNameAsync(string name)
        {
            var tenant = await TenantStore.FindAsync(name).ConfigureAwait(false);

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
        
        public async Task<FindTenantResultDto> FindTenantByIdAsync(Guid id)
        {
            var tenant = await TenantStore.FindAsync(id).ConfigureAwait(false);

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