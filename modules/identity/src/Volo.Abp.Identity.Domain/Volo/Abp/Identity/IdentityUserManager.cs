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
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity.Organizations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Settings;
using Volo.Abp.Identity.Settings;

namespace Volo.Abp.Identity
{
    public class IdentityUserManager : UserManager<IdentityUser>, IDomainService
    {
        protected override CancellationToken CancellationToken => _cancellationTokenProvider.Token;

        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        protected IOrganizationUnitRepository _organizationUnitRepository { get; private set; }
        protected IIdentityUserRepository _identityUserRepository { get; private set; }
        private readonly ISettingProvider _settingProvider;

        public IdentityUserManager(
            IdentityUserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<IdentityUserManager> logger,
            ICancellationTokenProvider cancellationTokenProvider,
            IOrganizationUnitRepository organizationUnitRepository,
            IIdentityUserRepository identityUserRepository,
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
            _cancellationTokenProvider = cancellationTokenProvider;
            _organizationUnitRepository = organizationUnitRepository;
            _identityUserRepository = identityUserRepository;
            _settingProvider = settingProvider;
        }

        public virtual async Task<IdentityUser> GetByIdAsync(Guid id)
        {
            var user = await Store.FindByIdAsync(id.ToString(), CancellationToken).ConfigureAwait(false);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), id);
            }

            return user;
        }

        public virtual async Task<IdentityResult> SetRolesAsync([NotNull] IdentityUser user, [NotNull] IEnumerable<string> roleNames)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNull(roleNames, nameof(roleNames));

            var currentRoleNames = await GetRolesAsync(user).ConfigureAwait(false);

            var result = await RemoveFromRolesAsync(user, currentRoleNames.Except(roleNames).Distinct()).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return result;
            }

            result = await AddToRolesAsync(user, roleNames.Except(currentRoleNames).Distinct()).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return result;
            }

            return IdentityResult.Success;
        }

        public virtual async Task<bool> IsInOrganizationUnitAsync(Guid userId, Guid ouId)
        {
            return await IsInOrganizationUnitAsync(
                await GetByIdAsync(userId),
                await _organizationUnitRepository.GetAsync(ouId)
                );
        }

        public virtual Task<bool> IsInOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
        {
            return Task.FromResult(user.IsInOrganizationUnit(ou.Id));
        }

        public virtual async Task AddToOrganizationUnitAsync(Guid userId, Guid ouId)
        {
            await AddToOrganizationUnitAsync(
                await _identityUserRepository.GetAsync(userId, true),
                await _organizationUnitRepository.GetAsync(ouId)
                );
        }

        public virtual async Task AddToOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
        {
            await _identityUserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits, _cancellationTokenProvider.Token).ConfigureAwait(false);
            
            var currentOus = user.OrganizationUnits;

            if (currentOus.Any(cou => cou.Id == ou.Id))
            {
                return;
            }

            await CheckMaxUserOrganizationUnitMembershipCountAsync(user.TenantId, currentOus.Count + 1);

            user.AddOrganizationUnit(ou.Id);
        }

        public virtual async Task RemoveFromOrganizationUnitAsync(Guid userId, Guid ouId)
        {
            await RemoveFromOrganizationUnitAsync(
                await _identityUserRepository.GetAsync(userId, true),
                await _organizationUnitRepository.GetAsync(ouId)
                );
        }

        public virtual async Task RemoveFromOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
        {
            await _identityUserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits, _cancellationTokenProvider.Token).ConfigureAwait(false);

            user.RemoveOrganizationUnit(ou.Id);
        }

        public virtual async Task SetOrganizationUnitsAsync(Guid userId, params Guid[] organizationUnitIds)
        {
            await SetOrganizationUnitsAsync(
                await _identityUserRepository.GetAsync(userId, true),
                organizationUnitIds
                );
        }

        public virtual async Task SetOrganizationUnitsAsync(IdentityUser user, params Guid[] organizationUnitIds)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNull(organizationUnitIds, nameof(organizationUnitIds));

            await CheckMaxUserOrganizationUnitMembershipCountAsync(user.TenantId, organizationUnitIds.Length);

            var currentOus = await _identityUserRepository.GetOrganizationUnitsAsync(user.Id).ConfigureAwait(false);

            //Remove from removed OUs
            foreach (var currentOu in currentOus)
            {
                if (!organizationUnitIds.Contains(currentOu.Id))
                {
                    await RemoveFromOrganizationUnitAsync(user.Id, currentOu.Id);
                }
            }

            //Add to added OUs
            foreach (var organizationUnitId in organizationUnitIds)
            {
                if (currentOus.All(ou => ou.Id != organizationUnitId))
                {
                    await AddToOrganizationUnitAsync(
                        user,
                        await _organizationUnitRepository.GetAsync(organizationUnitId)
                        );
                }
            }
        }

        private async Task CheckMaxUserOrganizationUnitMembershipCountAsync(Guid? tenantId, int requestedCount)
        {
            var maxCount = await _settingProvider.GetAsync<int>(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount).ConfigureAwait(false);
            if (requestedCount > maxCount)
            {
                throw new AbpException(string.Format("Can not set more than {0} organization unit for a user!", maxCount));
            }
        }

        [UnitOfWork]
        public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(IdentityUser user)
        {
            await _identityUserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits, _cancellationTokenProvider.Token).ConfigureAwait(false);

            var ouOfUser = user.OrganizationUnits;

            return await _organizationUnitRepository.GetListAsync(ouOfUser.Select(t => t.OrganizationUnitId));
        }
    }
}
