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
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AbpProfileService(
            IdentityUserManager userManager,
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
            IUnitOfWorkManager unitOfWorkManager)
            : base(userManager, claimsFactory)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await base.GetProfileDataAsync(context);
                await uow.CompleteAsync();
            }
        }

        [UnitOfWork]
        public override async Task IsActiveAsync(IsActiveContext context)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await base.IsActiveAsync(context);
                await uow.CompleteAsync();
            }
        }
    }
}
