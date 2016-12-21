using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Identity
{
    public class IdentityUserManager : UserManager<IdentityUser>
    {
        public IdentityUserManager(
            IUserStore<IdentityUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<IdentityUserManager> logger)
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

        }
    }
}
