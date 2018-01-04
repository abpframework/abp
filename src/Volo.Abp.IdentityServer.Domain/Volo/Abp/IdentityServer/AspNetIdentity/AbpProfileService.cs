using System.Threading.Tasks;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Volo.Abp.IdentityServer.AspNetIdentity
{
    //TODO: Implement multi-tenancy as like in old ABP

    public class AbpProfileService : ProfileService<IdentityUser> 
    {
        public AbpProfileService(
            IdentityUserManager userManager,
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory
        ) : base(userManager, claimsFactory)
        {

        }

        [UnitOfWork]
        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await base.GetProfileDataAsync(context);
        }

        [UnitOfWork]
        public override async Task IsActiveAsync(IsActiveContext context)
        {
            await base.IsActiveAsync(context);
        }
    }
}
