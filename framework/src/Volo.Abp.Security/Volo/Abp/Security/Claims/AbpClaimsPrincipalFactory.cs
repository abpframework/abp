using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Security.Claims;

public class AbpClaimsPrincipalFactory : IAbpClaimsPrincipalFactory, ITransientDependency
{
    public static string AuthenticationType => "Abp.Application";

    protected IServiceProvider ServiceProvider { get; }
    protected AbpClaimsPrincipalFactoryOptions Options { get; }

    public AbpClaimsPrincipalFactory(
        IServiceProvider serviceProvider,
        IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimOptions)
    {
        ServiceProvider = serviceProvider;
        Options = abpClaimOptions.Value;
    }

    public virtual async Task<ClaimsPrincipal> CreateAsync(ClaimsPrincipal? existsClaimsPrincipal = null)
    {
        return await InternalCreateAsync(Options, existsClaimsPrincipal, false);
    }

    public virtual async Task<ClaimsPrincipal> CreateDynamicAsync(ClaimsPrincipal? existsClaimsPrincipal = null)
    {
        return await InternalCreateAsync(Options, existsClaimsPrincipal, true);
    }

    public virtual async Task<ClaimsPrincipal> InternalCreateAsync(AbpClaimsPrincipalFactoryOptions options, ClaimsPrincipal? existsClaimsPrincipal = null, bool isDynamic = false)
    {
        var claimsPrincipal = existsClaimsPrincipal ?? new ClaimsPrincipal(new ClaimsIdentity(
            AuthenticationType,
            AbpClaimTypes.UserName,
            AbpClaimTypes.Role));

        var context = new AbpClaimsPrincipalContributorContext(claimsPrincipal, ServiceProvider);

        if (!isDynamic)
        {
            foreach (var contributorType in options.Contributors)
            {
                var contributor = (IAbpClaimsPrincipalContributor)ServiceProvider.GetRequiredService(contributorType);
                await contributor.ContributeAsync(context);
            }
        }
        else
        {
            foreach (var contributorType in options.DynamicContributors)
            {
                var contributor = (IAbpDynamicClaimsPrincipalContributor)ServiceProvider.GetRequiredService(contributorType);
                await contributor.ContributeAsync(context);
            }
        }

        return context.ClaimsPrincipal;
    }
}
