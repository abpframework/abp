using System;
using System.Linq;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery;

public class AbpAntiForgeryClaimsPrincipalNormalizer : IAbpAntiForgeryClaimsPrincipalNormalizer, ITransientDependency
{
    public const string UserIdClaimIssuer = "AbpAntiForgery";

    protected virtual string NormalizedIssuer => UserIdClaimIssuer;

    public virtual ClaimsPrincipal Normalize(ClaimsPrincipal principal)
    {
        var normalized = new ClaimsPrincipal();

        foreach (var identity in principal.Identities)
        {
            normalized.AddIdentity(NormalizeIdentity(identity));
        }

        return normalized;
    }

    protected virtual ClaimsIdentity NormalizeIdentity(ClaimsIdentity identity)
    {
        return new ClaimsIdentity(
            identity.Claims.Select(NormalizeClaim),
            identity.AuthenticationType,
            identity.NameClaimType,
            identity.RoleClaimType)
        {
            Actor = identity.Actor,
            BootstrapContext = identity.BootstrapContext,
            Label = identity.Label
        };
    }

    protected virtual Claim NormalizeClaim(Claim claim)
    {
        var newClaim = new Claim(
            claim.Type,
            claim.Value,
            claim.ValueType,
            IsUserIdentifierClaim(claim.Type) ? NormalizedIssuer : claim.Issuer,
            claim.OriginalIssuer);

        foreach (var property in claim.Properties)
        {
            newClaim.Properties[property.Key] = property.Value;
        }

        return newClaim;
    }

    // The claim types DefaultClaimUidExtractor inspects, in priority order, to build the antiforgery user id.
    protected virtual bool IsUserIdentifierClaim(string claimType)
    {
        return string.Equals(claimType, AbpClaimTypes.UserId, StringComparison.Ordinal) ||
               string.Equals(claimType, "sub", StringComparison.Ordinal) ||
               string.Equals(claimType, ClaimTypes.NameIdentifier, StringComparison.Ordinal) ||
               string.Equals(claimType, ClaimTypes.Upn, StringComparison.Ordinal);
    }
}
