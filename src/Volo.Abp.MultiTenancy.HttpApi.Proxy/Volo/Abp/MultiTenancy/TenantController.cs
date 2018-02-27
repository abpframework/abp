using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.MultiTenancy
{
    [Controller]
    public class TenantController : ITenantAppService //TODO: Throws exception on validation if we inherit from Controller
    {
        private readonly ITenantAppService _service;

        public TenantController(ITenantAppService service)
        {
            _service = service;
        }

        public Task<TenantDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        public Task<PagedResultDto<TenantDto>> GetListAsync(GetTenantsInput input)
        {
            return _service.GetListAsync(input);
        }

        public Task<TenantDto> CreateAsync(TenantCreateDto input)
        {
            return _service.CreateAsync(input);
        }

        public Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
        {
            return _service.UpdateAsync(id, input);
        }

        public Task DeleteAsync(Guid id)
        {
            return _service.DeleteAsync(id);
        }
    }
}