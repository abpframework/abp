using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public class IdentityRoleAppService : ApplicationService, IIdentityRoleAppService
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

        public async Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRolesInput input)
        {
            var count = (int)await _roleRepository.GetCountAsync();
            var list = await _roleRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityRoleDto>(
                count,
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list)
            );
        }

        public async Task<List<IdentityRoleDto>> GetAllListAsync()
        {
            var list = await _roleRepository.GetListAsync();

            return ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list);
        }

        public async Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
        {
            var role = new IdentityRole(GuidGenerator.Create(), input.Name);

            await _roleManager.CreateAsync(role);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
        }

        public async Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
        {
            var role = await _roleManager.GetByIdAsync(id);

            await _roleManager.SetRoleNameAsync(role, input.Name);

            await _roleManager.UpdateAsync(role);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return;
            }

            await _roleManager.DeleteAsync(role);
        }
    }
}
