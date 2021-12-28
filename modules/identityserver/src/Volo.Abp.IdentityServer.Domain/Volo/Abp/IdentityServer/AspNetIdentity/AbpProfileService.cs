using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Principal;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.IdentityServer.AspNetIdentity;

public class AbpProfileService : ProfileService<IdentityUser>
{
    protected ICurrentTenant CurrentTenant { get; }

    protected IAbpClaimsPrincipalFactory AbpClaimsPrincipalFactory { get; }

    public AbpProfileService(
        IdentityUserManager userManager,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        ICurrentTenant currentTenant,
        IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory)
        : base(userManager, claimsFactory)
    {
        CurrentTenant = currentTenant;
        AbpClaimsPrincipalFactory = abpClaimsPrincipalFactory;
    }

    [UnitOfWork]
    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        using (CurrentTenant.Change(context.Subject.FindTenantId()))
        {
            await base.GetProfileDataAsync(context);
        }
    }

    [UnitOfWork]
    public override async Task IsActiveAsync(IsActiveContext context)
    {
        using (CurrentTenant.Change(context.Subject.FindTenantId()))
        {
            await base.IsActiveAsync(context);
        }
    }

    [UnitOfWork]
    protected override async Task<ClaimsPrincipal> GetUserClaimsAsync(IdentityUser user)
    {
        var claimsPrincipal = await base.GetUserClaimsAsync(user);
        await AbpClaimsPrincipalFactory.DynamicCreateAsync(claimsPrincipal);
        return claimsPrincipal;
    }
}
