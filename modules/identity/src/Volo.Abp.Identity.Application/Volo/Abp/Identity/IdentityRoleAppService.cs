using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity
{
    [Authorize(IdentityPermissions.Roles.Default)]
    public class IdentityRoleAppService : IdentityAppServiceBase, IIdentityRoleAppService
    {
        private readonly IdentityRoleManager _roleManager;
        private readonly IIdentityRoleRepository _roleRepository;

        public IdentityRoleAppService(
            IdentityRoleManager roleManager,
            IIdentityRoleRepository roleRepository)
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
        }

        public async Task<IdentityRoleDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(
                await _roleManager.GetByIdAsync(id)
            );
        }

        public async Task<ListResultDto<IdentityRoleDto>> GetListAsync()
        {
            var list = await _roleRepository.GetListAsync();

            return new ListResultDto<IdentityRoleDto>(ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list));
        }

        [Authorize(IdentityPermissions.Roles.Create)]
        public async Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
        {
            var role = new IdentityRole(GuidGenerator.Create(), input.Name, CurrentTenant.Id);

            role.IsDefault = input.IsDefault;
            role.IsPublic = input.IsPublic;

            (await _roleManager.CreateAsync(role)).CheckErrors();
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
        }

        [Authorize(IdentityPermissions.Roles.Update)]
        public async Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
        {
            var role = await _roleManager.GetByIdAsync(id);
            role.ConcurrencyStamp = input.ConcurrencyStamp;

            (await _roleManager.SetRoleNameAsync(role, input.Name)).CheckErrors();

            role.IsDefault = input.IsDefault;
            role.IsPublic = input.IsPublic;

            (await _roleManager.UpdateAsync(role)).CheckErrors();
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
        }

        [Authorize(IdentityPermissions.Roles.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return;
            }

            (await _roleManager.DeleteAsync(role)).CheckErrors();
        }
    }
}
