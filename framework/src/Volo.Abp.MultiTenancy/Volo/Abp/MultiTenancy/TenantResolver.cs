using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy;

public class TenantResolver : ITenantResolver, ITransientDependency
{
    public ILogger<TenantResolver> Logger { get; set; }

    protected IServiceProvider ServiceProvider { get; }
    protected AbpTenantResolveOptions Options { get; }

    public TenantResolver(IOptions<AbpTenantResolveOptions> options, IServiceProvider serviceProvider)
    {
        Logger = NullLogger<TenantResolver>.Instance;

        ServiceProvider = serviceProvider;
        Options = options.Value;
    }

    public virtual async Task<TenantResolveResult> ResolveTenantIdOrNameAsync()
    {
        var result = new TenantResolveResult();

        Logger.LogDebug("Starting resolving tenant...");
        using (var serviceScope = ServiceProvider.CreateScope())
        {
            var context = new TenantResolveContext(serviceScope.ServiceProvider);

            foreach (var tenantResolver in Options.TenantResolvers)
            {
                Logger.LogDebug("Trying to resolve tenant through '{TenantResolverName}'...", tenantResolver.Name);
                await tenantResolver.ResolveAsync(context);

                result.AppliedResolvers.Add(tenantResolver.Name);

                if (context.HasResolvedTenantOrHost())
                {
                    result.TenantIdOrName = context.TenantIdOrName;
                    Logger.LogDebug("Tenant resolved by '{TenantResolverName}' as '{TenantIdOrName}'.", tenantResolver.Name, result.TenantIdOrName ?? "Host");
                    break;
                }
            }
        }

        if (result.TenantIdOrName.IsNullOrEmpty() && !string.IsNullOrWhiteSpace(Options.FallbackTenant))
        {
            result.TenantIdOrName = Options.FallbackTenant;
            result.AppliedResolvers.Add(TenantResolverNames.FallbackTenant);
            Logger.LogDebug("No tenant resolved. Using fallback tenant as '{FallbackTenant}'.", result.TenantIdOrName);
        }
        else if (result.TenantIdOrName.IsNullOrEmpty())
        {
            Logger.LogDebug("No tenant resolved.");
        }

        return result;
    }
}
