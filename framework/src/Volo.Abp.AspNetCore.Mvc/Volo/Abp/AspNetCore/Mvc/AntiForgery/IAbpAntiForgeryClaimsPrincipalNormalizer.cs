using System.Security.Claims;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery;

public interface IAbpAntiForgeryClaimsPrincipalNormalizer
{
    // Returns a copy of the principal whose user identifier claims carry a stable issuer, so the
    // antiforgery token's per-user identifier is the same across authentication schemes.
    ClaimsPrincipal Normalize(ClaimsPrincipal principal);
}
