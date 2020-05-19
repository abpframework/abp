using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public class IdentityUserRepositoryExternalUserLookupServiceProvider : IExternalUserLookupServiceProvider, ITransientDependency
    {
        protected IIdentityUserRepository UserRepository { get; }
        protected ILookupNormalizer LookupNormalizer { get; }

        public IdentityUserRepositoryExternalUserLookupServiceProvider(
            IIdentityUserRepository userRepository, 
            ILookupNormalizer lookupNormalizer)
        {
            UserRepository = userRepository;
            LookupNormalizer = lookupNormalizer;
        }

        public virtual async Task<IUserData> FindByIdAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            return (
                    await UserRepository.FindAsync(
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
                    await UserRepository.FindByNormalizedUserNameAsync(
                        LookupNormalizer.NormalizeName(userName),
                        includeDetails: false,
                        cancellationToken: cancellationToken
                    )
                )?.ToAbpUserData();
        }

        public virtual async Task<List<IUserData>> SearchAsync(
            string sorting,
            string filter, 
            int maxResultCount,
            CancellationToken cancellationToken = default)
        {
            var users = await UserRepository.GetListAsync(
                sorting: sorting,
                maxResultCount: maxResultCount,
                filter: filter,
                includeDetails: false,
                cancellationToken: cancellationToken
            );

            return users.Select(u => u.ToAbpUserData()).ToList();
        }
    }
}
