using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity
{
    [RemoteService]
    [Area("identity")]
    [ControllerName("Role")]
    [Route("api/identity/role")]
    public class IdentityRoleController : AbpController, IIdentityRoleAppService
    {
        private readonly IIdentityRoleAppService _roleAppService;

        public IdentityRoleController(IIdentityRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<IdentityRoleDto> GetAsync(Guid id)
        {
            return _roleAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRolesInput input)
        {
            return _roleAppService.GetListAsync(input);
        }

        [HttpPost]
        public virtual Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
        {
            return _roleAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
        {
            return _roleAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _roleAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("all")]
        public virtual Task<List<IdentityRoleDto>> GetAllListAsync()
        {
            return _roleAppService.GetAllListAsync();
        }
    }
}
