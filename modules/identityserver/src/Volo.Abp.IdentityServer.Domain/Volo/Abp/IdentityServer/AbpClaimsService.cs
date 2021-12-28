using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

using Volo.Abp.Security.Claims;

namespace Volo.Abp.IdentityServer;

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
