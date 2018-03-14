using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public class IdentityUserLookupService : IUserLookupService, ITransientDependency
    {
        private readonly IIdentityUserRepository _userRepository;
        private readonly ILookupNormalizer _lookupNormalizer;

        public IdentityUserLookupService(IIdentityUserRepository userRepository, ILookupNormalizer lookupNormalizer)
        {
            _userRepository = userRepository;
            _lookupNormalizer = lookupNormalizer;
        }

        public async Task<IUserInfo> FindByIdAsync(Guid id)
        {
            return (await _userRepository.FindAsync(id)).ToUserInfo();
        }

        public async Task<IUserInfo> FindByUserNameAsync(string userName)
        {
            return (await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.Normalize(userName))).ToUserInfo();
        }
    }
}
