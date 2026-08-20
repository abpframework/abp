using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity.AspNetCore;

[Dependency(ReplaceServices = true)]
public class FakeIdentitySessionChecker : IIdentitySessionChecker, ISingletonDependency
{
    public HashSet<string> RevokedSessionIds { get; } = new();

    public Task<bool> IsValidateAsync(string sessionId)
    {
        return Task.FromResult(!RevokedSessionIds.Contains(sessionId));
    }
}

public class TestSessionIdClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        if (identity != null && identity.FindSessionId() == null)
        {
            identity.AddClaim(new Claim(AbpClaimTypes.SessionId, Guid.NewGuid().ToString()));
        }

        return Task.CompletedTask;
    }
}
