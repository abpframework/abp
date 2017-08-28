using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
    }
}
