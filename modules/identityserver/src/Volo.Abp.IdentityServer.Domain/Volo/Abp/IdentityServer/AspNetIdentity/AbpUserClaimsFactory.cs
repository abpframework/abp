using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.IdentityServer.AspNetIdentity;

public class AbpUserClaimsFactory<TUser> : IUserClaimsPrincipalFactory<TUser>
    where TUser : class
{
    private readonly IObjectAccessor<IUserClaimsPrincipalFactory<TUser>> _inner;
    private readonly UserManager<TUser> _userManager;

    public AbpUserClaimsFactory(IObjectAccessor<IUserClaimsPrincipalFactory<TUser>> inner,
        UserManager<TUser> userManager)
    {
        _inner = inner;
        _userManager = userManager;
    }

    public async Task<ClaimsPrincipal> CreateAsync(TUser user)
    {
        var principal = await _inner.Value.CreateAsync(user);
        var identity = principal.Identities.First();

        if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Subject))
        {
            var sub = await _userManager.GetUserIdAsync(user);
            identity.AddIfNotContains(new Claim(JwtClaimTypes.Subject, sub));
        }

        return principal;
    }
}
