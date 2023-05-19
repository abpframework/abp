using System;
using System.Security.Claims;
using JetBrains.Annotations;

namespace Volo.Abp.Security.Claims;

public class AbpClaimsPrincipalContributorContext
{
    [NotNull]
    public ClaimsPrincipal ClaimsPrincipal { get; }

    [NotNull]
    public IServiceProvider ServiceProvider { get; }

    public AbpClaimsPrincipalContributorContext(
        [NotNull] ClaimsPrincipal claimsIdentity,
        [NotNull] IServiceProvider serviceProvider)
    {
        ClaimsPrincipal = claimsIdentity;
        ServiceProvider = serviceProvider;
    }
}
