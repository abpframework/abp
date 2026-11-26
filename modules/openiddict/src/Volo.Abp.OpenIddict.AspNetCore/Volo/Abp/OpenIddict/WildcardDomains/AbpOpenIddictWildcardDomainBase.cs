using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public abstract class AbpOpenIddictWildcardDomainBase<THandler, TOriginalHandler, TContext> : IOpenIddictServerHandler<TContext>
    where THandler : class
    where TOriginalHandler : class
    where TContext : OpenIddictServerEvents.BaseContext
{
    public ILogger<THandler> Logger { get; set; }
    protected TOriginalHandler OriginalHandler { get; set; }
    protected AbpOpenIddictWildcardDomainValidator WildcardDomainValidator { get; }

    protected AbpOpenIddictWildcardDomainBase(TOriginalHandler originalHandler, AbpOpenIddictWildcardDomainValidator wildcardDomainValidator)
    {
        OriginalHandler = originalHandler;
        WildcardDomainValidator = wildcardDomainValidator;

        Logger = NullLogger<THandler>.Instance;
    }

    public abstract ValueTask HandleAsync(TContext context);
}
