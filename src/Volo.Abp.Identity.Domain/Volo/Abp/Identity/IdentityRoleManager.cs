using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace Volo.Abp.Identity
{
    public class IdentityRoleManager : RoleManager<IdentityRole>, IDomainService
    {
        public IdentityRoleManager(
            IdentityRoleStore store,
            IEnumerable<IRoleValidator<IdentityRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<IdentityRoleManager> logger)
            : base(
                  store, 
                  roleValidators, 
                  keyNormalizer, 
                  errors, 
                  logger)
        {
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
    }
}
