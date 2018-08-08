using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.TenantManagement
{
    [Controller]
    [RemoteService]
    [Area("multi-tenancy")]
    public class TenantController : AbpController, ITenantAppService //TODO: Throws exception on validation if we inherit from Controller
    {
        private readonly ITenantAppService _service;

        public TenantController(ITenantAppService service)
        {
            _service = service;
        }

        public virtual Task<TenantDto> GetAsync(Guid id)
        {
            return _service.GetAsync(id);
        }

        public virtual Task<PagedResultDto<TenantDto>> GetListAsync(GetTenantsInput input)
        {
            return _service.GetListAsync(input);
        }

        public virtual Task<TenantDto> CreateAsync(TenantCreateDto input)
        {
            return _service.CreateAsync(input);
        }

        public virtual Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
        {
            return _service.UpdateAsync(id, input);
        }

        public virtual Task DeleteAsync(Guid id)
        {
            return _service.DeleteAsync(id);
        }
    }
}