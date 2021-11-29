using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Settings;
using Volo.Abp.Identity.Settings;

namespace Volo.Abp.Identity
{
    public class IdentityUserManager : UserManager<IdentityUser>, IDomainService
    {
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
        protected ISettingProvider SettingProvider { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        protected override CancellationToken CancellationToken => CancellationTokenProvider.Token;

        public IdentityUserManager(
            IdentityUserStore store,
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<IdentityUserManager> logger,
            ICancellationTokenProvider cancellationTokenProvider,
            IOrganizationUnitRepository organizationUnitRepository,
            ISettingProvider settingProvider)
            : base(
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger)
        {
            OrganizationUnitRepository = organizationUnitRepository;
            SettingProvider = settingProvider;
            RoleRepository = roleRepository;
            UserRepository = userRepository;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        public virtual async Task<IdentityResult> CreateAsync(IdentityUser user, string password, bool validatePassword)
        {
            var result = await UpdatePasswordHash(user, password, validatePassword);
            if (!result.Succeeded)
            {
                return result;
            }

            return await CreateAsync(user);
        }

        public virtual async Task<IdentityUser> GetByIdAsync(Guid id)
        {
            var user = await Store.FindByIdAsync(id.ToString(), CancellationToken);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), id);
            }

            return user;
        }

        public virtual async Task<IdentityResult> SetRolesAsync([NotNull] IdentityUser user,
            [NotNull] IEnumerable<string> roleNames)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNull(roleNames, nameof(roleNames));

            var currentRoleNames = await GetRolesAsync(user);

            var result = await RemoveFromRolesAsync(user, currentRoleNames.Except(roleNames).Distinct());
            if (!result.Succeeded)
            {
                return result;
            }

            result = await AddToRolesAsync(user, roleNames.Except(currentRoleNames).Distinct());
            if (!result.Succeeded)
            {
                return result;
            }

            return IdentityResult.Success;
        }

        public virtual async Task<bool> IsInOrganizationUnitAsync(Guid userId, Guid ouId)
        {
            var user = await UserRepository.GetAsync(userId, cancellationToken: CancellationToken);
            return user.IsInOrganizationUnit(ouId);
        }

        public virtual async Task<bool> IsInOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
        {
            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits,
                CancellationTokenProvider.Token);
            return user.IsInOrganizationUnit(ou.Id);
        }

        public virtual async Task AddToOrganizationUnitAsync(Guid userId, Guid ouId)
        {
            await AddToOrganizationUnitAsync(
                await UserRepository.GetAsync(userId, cancellationToken: CancellationToken),
                await OrganizationUnitRepository.GetAsync(ouId, cancellationToken: CancellationToken)
            );
        }

        public virtual async Task AddToOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
        {
            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits,
                CancellationTokenProvider.Token);

            if (user.OrganizationUnits.Any(cou => cou.OrganizationUnitId == ou.Id))
            {
                return;
            }

            await CheckMaxUserOrganizationUnitMembershipCountAsync(user.OrganizationUnits.Count + 1);

            user.AddOrganizationUnit(ou.Id);
            await UserRepository.UpdateAsync(user, cancellationToken: CancellationToken);
        }

        public virtual async Task RemoveFromOrganizationUnitAsync(Guid userId, Guid ouId)
        {
            var user = await UserRepository.GetAsync(userId, cancellationToken: CancellationToken);
            user.RemoveOrganizationUnit(ouId);
            await UserRepository.UpdateAsync(user, cancellationToken: CancellationToken);
        }

        public virtual async Task RemoveFromOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
        {
            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits,
                CancellationTokenProvider.Token);

            user.RemoveOrganizationUnit(ou.Id);
            await UserRepository.UpdateAsync(user, cancellationToken: CancellationToken);
        }

        public virtual async Task SetOrganizationUnitsAsync(Guid userId, params Guid[] organizationUnitIds)
        {
            await SetOrganizationUnitsAsync(
                await UserRepository.GetAsync(userId, cancellationToken: CancellationToken),
                organizationUnitIds
            );
        }

        public virtual async Task SetOrganizationUnitsAsync(IdentityUser user, params Guid[] organizationUnitIds)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNull(organizationUnitIds, nameof(organizationUnitIds));

            await CheckMaxUserOrganizationUnitMembershipCountAsync(organizationUnitIds.Length);

            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits,
                CancellationTokenProvider.Token);

            //Remove from removed OUs
            foreach (var ouId in user.OrganizationUnits.Select(uou => uou.OrganizationUnitId).ToArray())
            {
                if (!organizationUnitIds.Contains(ouId))
                {
                    user.RemoveOrganizationUnit(ouId);
                }
            }

            //Add to added OUs
            foreach (var organizationUnitId in organizationUnitIds)
            {
                if (!user.IsInOrganizationUnit(organizationUnitId))
                {
                    user.AddOrganizationUnit(organizationUnitId);
                }
            }

            await UserRepository.UpdateAsync(user, cancellationToken: CancellationToken);
        }

        private async Task CheckMaxUserOrganizationUnitMembershipCountAsync(int requestedCount)
        {
            var maxCount =
                await SettingProvider.GetAsync<int>(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount);
            if (requestedCount > maxCount)
            {
                throw new BusinessException(IdentityErrorCodes.MaxAllowedOuMembership)
                    .WithData("MaxUserMembershipCount", maxCount);
            }
        }

        [UnitOfWork]
        public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(IdentityUser user,
            bool includeDetails = false)
        {
            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits,
                CancellationTokenProvider.Token);

            return await OrganizationUnitRepository.GetListAsync(
                user.OrganizationUnits.Select(t => t.OrganizationUnitId),
                includeDetails,
                cancellationToken: CancellationToken
            );
        }

        [UnitOfWork]
        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitAsync(
            OrganizationUnit organizationUnit,
            bool includeChildren = false)
        {
            if (includeChildren)
            {
                return await UserRepository
                    .GetUsersInOrganizationUnitWithChildrenAsync(organizationUnit.Code, CancellationToken);
            }
            else
            {
                return await UserRepository
                    .GetUsersInOrganizationUnitAsync(organizationUnit.Id, CancellationToken);
            }
        }

        public virtual async Task<IdentityResult> AddDefaultRolesAsync([NotNull] IdentityUser user)
        {
            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, CancellationToken);

            foreach (var role in await RoleRepository.GetDefaultOnesAsync(cancellationToken: CancellationToken))
            {
                if (!user.IsInRole(role.Id))
                {
                    user.AddRole(role.Id);
                }
            }

            return await UpdateUserAsync(user);
        }
    }
}
