using System.Threading.Tasks;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Volo.Abp.IdentityServer.AspNetIdentity
{
    public class AbpResourceOwnerPasswordValidator : ResourceOwnerPasswordValidator<IdentityUser>
    {
        public AbpResourceOwnerPasswordValidator(
            IdentityUserManager userManager, 
            SignInManager<IdentityUser> signInManager, 
            IEventService events, 
            ILogger<ResourceOwnerPasswordValidator<IdentityUser>> logger
            ) : base(
                userManager, 
                signInManager, 
                events, 
                logger)
        {
        }

        [UnitOfWork]
        public override async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            await base.ValidateAsync(context);
        }
    }
}
