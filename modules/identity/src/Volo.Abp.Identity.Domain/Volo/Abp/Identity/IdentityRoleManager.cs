using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity
{
    public class IdentityRoleManager : RoleManager<IdentityRole>, IDomainService
    {
        protected override CancellationToken CancellationToken => _cancellationTokenProvider.Token;

        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public IdentityRoleManager(
            IdentityRoleStore store,
            IEnumerable<IRoleValidator<IdentityRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<IdentityRoleManager> logger, 
            ICancellationTokenProvider cancellationTokenProvider)
            : base(
                  store, 
                  roleValidators, 
                  keyNormalizer, 
                  errors, 
                  logger)
        {
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public async Task<IdentityRole> GetByIdAsync(Guid id)
        {
            var role = await Store.FindByIdAsync(id.ToString(), CancellationToken);
            if (role == null)
            {
                throw new EntityNotFoundException(typeof(IdentityRole), id);
            }

            return role;
        }

        public List<IdentityRole> GetDefaultRoles()
        {
            if (Roles == null)
            {
                return new List<IdentityRole>();
            }

            return Roles.Where(r => r.IsDefault).ToList();
        }

        public override async Task<IdentityResult> SetRoleNameAsync(IdentityRole role, string name)
        {
            if (role.IsStatic && role.Name != name)
            {
                throw new AbpException("Static roles can not be renamed."); // TODO: localize & change exception type
            }

            return await base.SetRoleNameAsync(role,name);
        }

        public override async Task<IdentityResult> DeleteAsync(IdentityRole role)
        {
            if (role.IsStatic)
            {
                throw new AbpException("Static roles can not be deleted."); // TODO: localize & change exception type
            }

            return await base.DeleteAsync(role);
        }
    }
}
