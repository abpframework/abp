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
                identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
            }

            if (CurrentUser.ImpersonatorId != user.Id || CurrentTenant.ImpersonatorId != user.TenantId)
            {
                if (CurrentUser.ImpersonatorId.HasValue)
                {
                    identity.AddClaim(new Claim(AbpClaimTypes.UserImpersonatorId, CurrentUser.ImpersonatorId.ToString()));
                }

                if (CurrentTenant.ImpersonatorId.HasValue)
                {
                    identity.AddClaim(new Claim(AbpClaimTypes.TenantImpersonatorId, CurrentTenant.ImpersonatorId.ToString()));
                }
            }

            return principal;
        }
    }
}
