using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;

namespace Volo.Abp.OpenIddict.Tokens;

public class AbpTokenManager : OpenIddictTokenManager<OpenIddictTokenModel>
{
    protected AbpOpenIddictIdentifierConverter IdentifierConverter { get; }

    public AbpTokenManager(
        [NotNull] [ItemNotNull] IOpenIddictTokenCache<OpenIddictTokenModel> cache,
        [NotNull] [ItemNotNull] ILogger<OpenIddictTokenManager<OpenIddictTokenModel>> logger,
        [NotNull] [ItemNotNull] IOptionsMonitor<OpenIddictCoreOptions> options,
        [NotNull] IOpenIddictTokenStoreResolver resolver,
        AbpOpenIddictIdentifierConverter identifierConverter)
        : base(cache, logger, options, resolver)
    {
        IdentifierConverter = identifierConverter;
    }

    public async override ValueTask UpdateAsync(OpenIddictTokenModel token, CancellationToken cancellationToken = default)
    {
        if (!Options.CurrentValue.DisableEntityCaching)
        {
            var entity = await Store.FindByIdAsync(IdentifierConverter.ToString(token.Id), cancellationToken);
            if (entity != null)
            {
                await Cache.RemoveAsync(entity, cancellationToken);
            }
        }

        await base.UpdateAsync(token, cancellationToken);
    }
}
