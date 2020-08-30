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
        protected ICurrentImpersonatorUser CurrentImpersonatorUser { get; }
        protected ICurrentImpersonatorTenant CurrentImpersonatorTenant  { get; }

        public AbpUserClaimsPrincipalFactory(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options,
            ICurrentImpersonatorUser currentImpersonatorUser,
            ICurrentImpersonatorTenant currentImpersonatorTenant)
            : base(
                  userManager,
                  roleManager,
                  options)
        {
            CurrentImpersonatorUser = currentImpersonatorUser;
            CurrentImpersonatorTenant = currentImpersonatorTenant;
        }

        [UnitOfWork]
        public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await base.CreateAsync(user);

            if (user.TenantId.HasValue)
            {
                principal.Identities
                    .First()
                    .AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));
            }

            if (CurrentImpersonatorUser.Id != user.Id || CurrentImpersonatorTenant.Id != user.TenantId)
            {
                if (CurrentImpersonatorUser.Id.HasValue)
                {
                    principal.Identities
                        .First()
                        .AddClaim(new Claim(AbpClaimTypes.ImpersonatorUserId, CurrentImpersonatorUser.Id.ToString()));
                }

                if (CurrentImpersonatorTenant.Id.HasValue)
                {
                    principal.Identities
                        .First()
                        .AddClaim(new Claim(AbpClaimTypes.ImpersonatorTenantId, CurrentImpersonatorTenant.Id.ToString()));
                }
            }

            return principal;
        }
    }
}
