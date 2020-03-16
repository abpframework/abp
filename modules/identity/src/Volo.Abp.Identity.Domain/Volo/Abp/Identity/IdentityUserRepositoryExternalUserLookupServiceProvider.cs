using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public class IdentityUserRepositoryExternalUserLookupServiceProvider : IExternalUserLookupServiceProvider, ITransientDependency
    {
        private readonly IIdentityUserRepository _userRepository;
        private readonly ILookupNormalizer _lookupNormalizer;

        public IdentityUserRepositoryExternalUserLookupServiceProvider(
            IIdentityUserRepository userRepository, 
            ILookupNormalizer lookupNormalizer)
        {
            _userRepository = userRepository;
            _lookupNormalizer = lookupNormalizer;
        }

        public virtual async Task<IUserData> FindByIdAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            return (
                await _userRepository.FindAsync(
                    id,
                    includeDetails: false,
                    cancellationToken: cancellationToken
                )
)?.ToAbpUserData();
        }

        public virtual async Task<IUserData> FindByUserNameAsync(
            string userName, 
            CancellationToken cancellationToken = default)
        {
            return (
                await _userRepository.FindByNormalizedUserNameAsync(
                    _lookupNormalizer.NormalizeName(userName),
                    includeDetails: false,
                    cancellationToken: cancellationToken
                )
)?.ToAbpUserData();
        }
    }
}
