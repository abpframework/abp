using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity
{
    public class UserRoleFinder : IUserRoleFinder, ITransientDependency
    {
        private readonly IIdentityUserRepository _identityUserRepository;

        public UserRoleFinder(IIdentityUserRepository identityUserRepository)
        {
            _identityUserRepository = identityUserRepository;
        }

        public virtual async Task<string[]> GetRolesAsync(Guid userId)
        {
            return (await _identityUserRepository.GetRoleNamesAsync(userId)).ToArray();
        }
    }
}
