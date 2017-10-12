using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity
{
    public class AbpSecurityStampValidator : SecurityStampValidator<IdentityUser>
    {
        public AbpSecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options,
            SignInManager<IdentityUser> signInManager,
            ISystemClock systemClock)
            : base(
                options, 
                signInManager,
                systemClock)
        {
        }

        [UnitOfWork]
        public override Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            return base.ValidateAsync(context);
        }
    }
}
