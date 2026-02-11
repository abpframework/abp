using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Security.Claims;

public class AbpClaimsTransformation : IClaimsTransformation
{
    protected IOptions<AbpClaimsMapOptions> AbpClaimsMapOptions { get; }

    public AbpClaimsTransformation(IOptions<AbpClaimsMapOptions> abpClaimsMapOptions)
    {
        AbpClaimsMapOptions = abpClaimsMapOptions;
    }

    public virtual Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var mapClaims = principal.Claims.Where(claim => AbpClaimsMapOptions.Value.Maps.Keys.Contains(claim.Type));

        principal.AddIdentity(new ClaimsIdentity(mapClaims.Select(
                    claim => new Claim(
                        AbpClaimsMapOptions.Value.Maps[claim.Type](),
                        claim.Value,
                        claim.ValueType,
                        claim.Issuer
                    )
                )
            )
        );

        return Task.FromResult(principal);
    }
}
