using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Identity;

// ReSharper disable once CheckNamespace
namespace Volo.Abp.Identity.ClientProxies
{
    public partial class IdentityRoleClientProxy
    {
        public virtual async Task<ListResultDto<IdentityRoleDto>> GetAllListAsync()
        {
            return await RequestAsync<ListResultDto<IdentityRoleDto>>(nameof(GetAllListAsync));
        }

        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRolesInput input)
        {
            return await RequestAsync<PagedResultDto<IdentityRoleDto>>(nameof(GetListAsync), input);
        }

        public virtual async Task<IdentityRoleDto> GetAsync(Guid id)
        {
            return await RequestAsync<IdentityRoleDto>(nameof(GetAsync), id);
        }

        public virtual async Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
        {
            return await RequestAsync<IdentityRoleDto>(nameof(CreateAsync), input);
        }

        public virtual async Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
        {
            return await RequestAsync<IdentityRoleDto>(nameof(UpdateAsync), id, input);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await RequestAsync(nameof(DeleteAsync), id);
        }

    }
}
