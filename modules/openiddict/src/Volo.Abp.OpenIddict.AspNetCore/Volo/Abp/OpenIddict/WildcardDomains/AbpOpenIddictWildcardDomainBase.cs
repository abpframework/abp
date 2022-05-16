using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenIddict.Server;
using Volo.Abp.Text.Formatting;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public abstract class AbpOpenIddictWildcardDomainBase<THandler, TContext> : IOpenIddictServerHandler<TContext>
    where THandler : class, new()
    where TContext : OpenIddictServerEvents.BaseContext
{
    protected THandler Handler { get; set; }
    protected AbpOpenIddictWildcardDomainOptions WildcardDomainOptions { get; }

    protected AbpOpenIddictWildcardDomainBase(IOptions<AbpOpenIddictWildcardDomainOptions> wildcardDomainOptions, bool initHandler = true)
    {
        WildcardDomainOptions = wildcardDomainOptions.Value;
        if (initHandler)
        {
            Handler = new THandler();
        }
    }

    public abstract ValueTask HandleAsync(TContext context);

    protected virtual Task<bool> CheckWildcardDomainAsync(string url)
    {
        foreach (var domainFormat in WildcardDomainOptions.WildcardDomainsFormat)
        {
            var extractResult = FormattedStringValueExtracter.Extract(url, domainFormat, ignoreCase: true);
            if (extractResult.IsMatch)
            {
                return Task.FromResult(true);
            }
        }

        foreach (var domainFormat in WildcardDomainOptions.WildcardDomainsFormat)
        {
            if (domainFormat.Replace("{0}.", "").Equals(url, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }
}
