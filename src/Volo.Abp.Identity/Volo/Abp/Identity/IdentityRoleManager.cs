using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Identity
{
    public class IdentityRoleManager : RoleManager<IdentityRole>
    {
        public IdentityRoleManager(
            IRoleStore<IdentityRole> store,
            IEnumerable<IRoleValidator<IdentityRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<IdentityRoleManager> logger,
            IHttpContextAccessor contextAccessor)
            : base(
                  store, 
                  roleValidators, 
                  keyNormalizer, 
                  errors, 
                  logger, 
                  contextAccessor)
        {
        }
    }
}
