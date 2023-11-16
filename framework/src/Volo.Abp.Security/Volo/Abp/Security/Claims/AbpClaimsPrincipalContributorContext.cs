using System;
using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Security.Claims;

public class AbpClaimsPrincipalContributorContext
{
    [NotNull]
    public ClaimsPrincipal ClaimsPrincipal { get; set; }

    [NotNull]
    public IServiceProvider ServiceProvider { get; }

    public AbpClaimsPrincipalContributorContext(
        [NotNull] ClaimsPrincipal claimsIdentity,
        [NotNull] IServiceProvider serviceProvider)
    {
        ClaimsPrincipal = claimsIdentity;
        ServiceProvider = serviceProvider;
    }

    public virtual T GetRequiredService<T>()
        where T : notnull
    {
        Check.NotNull(ServiceProvider, nameof(ServiceProvider));
        return ServiceProvider.GetRequiredService<T>();
    }
}
