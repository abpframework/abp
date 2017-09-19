using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace Volo.Abp.Identity
{
    public class IdentityUserManager : UserManager<IdentityUser>, IDomainService
    {
        public IdentityUserManager(
            IdentityUserStore store,
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

        public async Task<IdentityUser> GetByIdAsync(Guid id)
        {
            var user = await Store.FindByIdAsync(id.ToString(), CancellationToken);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), id);
            }

            return user;
        }
    }
}
