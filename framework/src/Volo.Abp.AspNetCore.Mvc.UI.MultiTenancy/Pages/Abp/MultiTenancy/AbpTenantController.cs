using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;

namespace Pages.Abp.MultiTenancy
{
    [Route("api/abp/multi-tenancy")]
    public class AbpTenantController : AbpController, IAbpTenantAppService
    {
        private readonly IAbpTenantAppService _abpTenantAppService;

        public AbpTenantController(IAbpTenantAppService abpTenantAppService)
        {
            _abpTenantAppService = abpTenantAppService;
        }

        [HttpGet]
        [Route("find-tenant/{name}")] //TODO: Remove on v1.0
        [Route("tenants/by-name/{name}")]
        public async Task<FindTenantResultDto> FindTenantByNameAsync(string name)
        {
            return await _abpTenantAppService.FindTenantByNameAsync(name);
        }

        [HttpGet]
        [Route("tenants/by-id/{id}")]
        public async Task<FindTenantResultDto> FindTenantByIdAsync(Guid id)
        {
            return await _abpTenantAppService.FindTenantByIdAsync(id);
        }
    }
}
