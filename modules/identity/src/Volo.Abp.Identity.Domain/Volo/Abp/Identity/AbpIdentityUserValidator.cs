using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity
{
    public class AbpIdentityUserValidator : IUserValidator<IdentityUser>
    {
        protected IOptions<AbpMultiTenancyOptions> MultiTenancyOptions { get; }
        protected IAbpDistributedLock DistributedLock { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IDataFilter<IMultiTenant> TenantFilter { get; }
        protected IIdentityUserRepository UserRepository { get; }

        public AbpIdentityUserValidator(
            IOptions<AbpMultiTenancyOptions> multiTenancyOptions,
            IAbpDistributedLock distributedLock,
            ICurrentTenant currentTenant,
            IDataFilter<IMultiTenant> tenantFilter,
            IIdentityUserRepository userRepository)
        {
            MultiTenancyOptions = multiTenancyOptions;
            DistributedLock = distributedLock;
            CurrentTenant = currentTenant;
            TenantFilter = tenantFilter;
            UserRepository = userRepository;
        }

        public virtual async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            Check.NotNull(manager, nameof(manager));
            Check.NotNull(user, nameof(user));

            var defaultUserValidator = new UserValidator<IdentityUser>(manager.ErrorDescriber);
            return MultiTenancyOptions.Value.UserSharingStrategy == TenantUserSharingStrategy.Isolated
                ? await ValidateIsolatedUserAsync(manager, user, defaultUserValidator)
                : await ValidateSharedUserAsync(manager, user, defaultUserValidator);
        }

        protected virtual async Task<IdentityResult> ValidateIsolatedUserAsync(UserManager<IdentityUser> manager, IdentityUser user, UserValidator<IdentityUser> defaultValidator)
        {
            var errors = new List<IdentityError>();

            var defaultValidationResult = await defaultValidator.ValidateAsync(manager, user);
            if (!defaultValidationResult.Succeeded)
            {
                return defaultValidationResult;
            }

            var userName = await manager.GetUserNameAsync(user);
            if (userName == null)
            {
                errors.Add(manager.ErrorDescriber.InvalidUserName(null));
            }
            else
            {
                var owner = await manager.FindByEmailAsync(userName);
                if (owner != null && !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(manager.ErrorDescriber.InvalidUserName(userName));
                }
            }

            var email = await manager.GetEmailAsync(user);
            if (email == null)
            {
                errors.Add(manager.ErrorDescriber.InvalidEmail(null));
            }
            else
            {
                var owner = await manager.FindByNameAsync(email);
                if (owner != null && !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(manager.ErrorDescriber.InvalidEmail(email));
                }
            }

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }

        protected virtual async Task<IdentityResult> ValidateSharedUserAsync(UserManager<IdentityUser> manager, IdentityUser user, UserValidator<IdentityUser> defaultValidator)
        {
            var errors = new List<IdentityError>();

            using (CurrentTenant.Change(user.TenantId))
            {
                var defaultValidationResult = await defaultValidator.ValidateAsync(manager, user);
                if (!defaultValidationResult.Succeeded)
                {
                    return defaultValidationResult;
                }
            }

            await using var handle = await DistributedLock.TryAcquireAsync(nameof(AbpIdentityUserValidator), TimeSpan.FromMinutes(1));
            if (handle == null)
            {
                throw new AbpException("Could not acquire distributed lock for validating user uniqueness for shared user sharing strategy!");
            }

            using (CurrentTenant.Change(null))
            {
                using (TenantFilter.Disable())
                {
                    IdentityUser owner;
                    using (CurrentTenant.Change(user.TenantId))
                    {
                        owner = await manager.FindByIdAsync(user.Id.ToString());
                    }

                    var normalizedUserName = manager.NormalizeName(user.UserName);
                    var normalizedEmail = manager.NormalizeEmail(user.Email);

                    var users = (await UserRepository.GetUsersByNormalizedUserNamesAsync([normalizedUserName!, normalizedEmail!], true)).Where(x => x.Id != user.Id).ToList();
                    var usersByUserName = users.Where(x => x.NormalizedUserName == normalizedUserName).ToList();
                    if (owner != null)
                    {
                        usersByUserName.RemoveAll(x => x.NormalizedUserName == user.NormalizedUserName);
                    }
                    if (usersByUserName.Any())
                    {
                        errors.Add(manager.ErrorDescriber.DuplicateUserName(user.UserName!));
                    }

                    var usersByEmail = users.Where(x => x.NormalizedUserName == normalizedEmail).ToList();
                    if (owner != null)
                    {
                        usersByEmail.RemoveAll(x => x.NormalizedEmail == user.NormalizedEmail);
                    }
                    if (usersByEmail.Any())
                    {
                        errors.Add(manager.ErrorDescriber.InvalidEmail(user.Email!));
                    }

                    users = await UserRepository.GetUsersByNormalizedEmailsAsync([normalizedEmail!, normalizedUserName!], true);
                    usersByEmail = users.Where(x => x.NormalizedEmail == normalizedEmail).ToList();
                    if (owner != null)
                    {
                        usersByEmail.RemoveAll(x => x.NormalizedEmail == user.NormalizedEmail);
                    }
                    if (usersByEmail.Any())
                    {
                        errors.Add(manager.ErrorDescriber.DuplicateEmail(user.Email!));
                    }

                    usersByUserName = users.Where(x => x.NormalizedEmail == normalizedUserName).ToList();
                    if (owner != null)
                    {
                        usersByUserName.RemoveAll(x => x.NormalizedUserName == user.NormalizedUserName);
                    }
                    if (usersByUserName.Any())
                    {
                        errors.Add(manager.ErrorDescriber.InvalidUserName(user.UserName!));
                    }
                }
            }

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
    }
}
