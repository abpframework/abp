using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public class AbpUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>, ITransientDependency
    {
        protected ICurrentUser CurrentUser { get; }
        protected ICurrentTenant CurrentTenant  { get; }

        public AbpUserClaimsPrincipalFactory(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant)
            : base(
                  userManager,
                  roleManager,
                  options)
        {
            CurrentUser = currentUser;
            CurrentTenant = currentTenant;
        }

        [UnitOfWork]
        public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = principal.Identities.First();

            if (user.TenantId.HasValue)
            {
                identity.AddIfNotContains(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
            }

            if (!user.Name.IsNullOrWhiteSpace())
            {
                identity.AddIfNotContains(new Claim(AbpClaimTypes.Name, user.Name));
            }
            if (!user.Surname.IsNullOrWhiteSpace())
            {
                identity.AddIfNotContains(new Claim(AbpClaimTypes.SurName, user.Surname));
            }

            if (!user.PhoneNumber.IsNullOrWhiteSpace())
            {
                identity.AddIfNotContains(new Claim(AbpClaimTypes.PhoneNumber, user.PhoneNumber));
            }
            identity.AddIfNotContains(new Claim(AbpClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed.ToString()));

            if (!user.Email.IsNullOrWhiteSpace())
            {
                identity.AddIfNotContains(new Claim(AbpClaimTypes.Email, user.Email));
            }
            identity.AddIfNotContains(new Claim(AbpClaimTypes.EmailVerified, user.EmailConfirmed.ToString()));

            return principal;
        }
    }
}
