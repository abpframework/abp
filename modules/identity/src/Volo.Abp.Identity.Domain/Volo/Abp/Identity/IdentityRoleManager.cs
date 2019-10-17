using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity
{
    public class IdentityRoleManager : RoleManager<IdentityRole>, IDomainService
    {
        protected override CancellationToken CancellationToken => _cancellationTokenProvider.Token;

        private readonly IStringLocalizer<IdentityResource> _localizer;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public IdentityRoleManager(
            IdentityRoleStore store,
            IEnumerable<IRoleValidator<IdentityRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<IdentityRoleManager> logger,
            IStringLocalizer<IdentityResource> localizer,
            ICancellationTokenProvider cancellationTokenProvider)
            : base(
                  store, 
                  roleValidators, 
                  keyNormalizer, 
                  errors, 
                  logger)
        {
            _localizer = localizer;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public virtual async Task<IdentityRole> GetByIdAsync(Guid id)
        {
            var role = await Store.FindByIdAsync(id.ToString(), CancellationToken);
            if (role == null)
            {
                throw new EntityNotFoundException(typeof(IdentityRole), id);
            }

            return role;
        }

        public override async Task<IdentityResult> SetRoleNameAsync(IdentityRole role, string name)
        {
            if (role.IsStatic && role.Name != name)
            {
                throw new BusinessException(_localizer["Identity.StaticRoleRenamingErrorMessage"]); // TODO: localize & change exception type
            }

            return await base.SetRoleNameAsync(role,name);
        }

        public override async Task<IdentityResult> DeleteAsync(IdentityRole role)
        {
            if (role.IsStatic)
            {
                throw new BusinessException(_localizer["Identity.StaticRoleDeletionErrorMessage"]); // TODO: localize & change exception type
            }

            return await base.DeleteAsync(role);
        }
    }
}
