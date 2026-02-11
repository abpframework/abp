using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Server;
using Volo.Abp.Http;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public abstract class AbpOpenIddictWildcardDomainBase<THandler, TOriginalHandler, TContext> : IOpenIddictServerHandler<TContext>
    where THandler : class
    where TOriginalHandler : class
    where TContext : OpenIddictServerEvents.BaseContext
{
    public ILogger<THandler> Logger { get; set; }
    protected TOriginalHandler OriginalHandler { get; set; }
    protected AbpOpenIddictWildcardDomainOptions WildcardDomainOptions { get; }

    protected AbpOpenIddictWildcardDomainBase(IOptions<AbpOpenIddictWildcardDomainOptions> wildcardDomainOptions, TOriginalHandler originalHandler)
    {
        WildcardDomainOptions = wildcardDomainOptions.Value;
        OriginalHandler = originalHandler;

        Logger = NullLogger<THandler>.Instance;
    }

    public abstract ValueTask HandleAsync(TContext context);

    protected virtual Task<bool> CheckWildcardDomainAsync(string url)
    {
        if (WildcardDomainOptions.WildcardDomainsFormat.IsNullOrEmpty())
        {
            Logger.LogDebug("No wildcard domain format configured.");
            return Task.FromResult(false);
        }

        Logger.LogDebug("Checking wildcard domain for url: {url}", url);
        foreach (var domain in WildcardDomainOptions.WildcardDomainsFormat.Select(domainFormat => domainFormat.Replace("{0}", "*")))
        {
            Logger.LogDebug("Checking wildcard domain format: {domain}", domain);
            if (UrlHelpers.IsSubdomainOf(url, domain))
            {
                Logger.LogDebug("The url: {url} is a wildcard domain of: {domain}", url, domain);
                return Task.FromResult(true);
            }
        }

        Logger.LogDebug("No wildcard domain found for url: {url}", url);
        return Task.FromResult(false);
    }
}
