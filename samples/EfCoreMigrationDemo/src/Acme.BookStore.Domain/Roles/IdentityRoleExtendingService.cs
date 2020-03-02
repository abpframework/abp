using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Acme.BookStore.Roles
{
    public class IdentityRoleExtendingService : ITransientDependency
    {
        private readonly IIdentityRoleRepository _identityRoleRepository;

        public IdentityRoleExtendingService(IIdentityRoleRepository identityRoleRepository)
        {
            _identityRoleRepository = identityRoleRepository;
        }

        public async Task<string> GetTitleAsync(Guid id)
        {
            var role = await _identityRoleRepository.GetAsync(id);

            return role.GetProperty<string>("Title");
        }

        public async Task SetTitleAsync(Guid id, string newTitle)
        {
            var role = await _identityRoleRepository.GetAsync(id);
            
            role.SetProperty("Title", newTitle);
            
            await _identityRoleRepository.UpdateAsync(role);
        }
    }
}
