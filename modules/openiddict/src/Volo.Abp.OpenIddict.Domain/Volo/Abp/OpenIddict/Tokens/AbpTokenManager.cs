using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Tokens;

public class AbpTokenManager : OpenIddictTokenManager<OpenIddictTokenModel>
{
    protected AbpOpenIddictIdentifierConverter IdentifierConverter { get; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public AbpTokenManager(
        [NotNull] [ItemNotNull] IOpenIddictTokenCache<OpenIddictTokenModel> cache,
        [NotNull] [ItemNotNull] ILogger<OpenIddictTokenManager<OpenIddictTokenModel>> logger,
        [NotNull] [ItemNotNull] IOptionsMonitor<OpenIddictCoreOptions> options,
        [NotNull] IOpenIddictTokenStore<OpenIddictTokenModel> resolver,
        AbpOpenIddictIdentifierConverter identifierConverter,
        IUnitOfWorkManager unitOfWorkManager)
        : base(cache, logger, options, resolver)
    {
        IdentifierConverter = identifierConverter;
        UnitOfWorkManager = unitOfWorkManager;
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

    public async override ValueTask<long> RevokeAsync(string subject, string client, string status, string type, CancellationToken cancellationToken = default)
    {
        var count = await base.RevokeAsync(subject, client, status, type, cancellationToken);

        if (count > 0)
        {
            // The revoked tokens no longer match the original status filter.
            await RemoveFromCacheAsync(ct => Store.FindAsync(subject, client, OpenIddictConstants.Statuses.Revoked, type, ct));
        }

        return count;
    }

    public async override ValueTask<long> RevokeByApplicationIdAsync(string identifier, CancellationToken cancellationToken = default)
    {
        var count = await base.RevokeByApplicationIdAsync(identifier, cancellationToken);
        if (count > 0)
        {
            await RemoveFromCacheAsync(ct => Store.FindByApplicationIdAsync(identifier, ct));
        }
        return count;
    }

    public async override ValueTask<long> RevokeByAuthorizationIdAsync(string identifier, CancellationToken cancellationToken = default)
    {
        var count = await base.RevokeByAuthorizationIdAsync(identifier, cancellationToken);
        if (count > 0)
        {
            await RemoveFromCacheAsync(ct => Store.FindByAuthorizationIdAsync(identifier, ct));
        }
        return count;
    }

    public async override ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        var count = await base.RevokeBySubjectAsync(subject, cancellationToken);
        if (count > 0)
        {
            await RemoveFromCacheAsync(ct => Store.FindBySubjectAsync(subject, ct));
        }
        return count;
    }

    protected virtual async Task RemoveFromCacheAsync(Func<CancellationToken, IAsyncEnumerable<OpenIddictTokenModel>> tokens)
    {
        if (Options.CurrentValue.DisableEntityCaching)
        {
            return;
        }

        // Inside an outer transactional unit of work the revoke is not committed yet, so the cache is cleared after the unit of work is completed.
        var unitOfWork = UnitOfWorkManager.Current;
        if (unitOfWork != null && unitOfWork.Options.IsTransactional)
        {
            unitOfWork.OnCompleted(() => RemoveTokensFromCacheAsync(tokens));
            return;
        }

        await RemoveTokensFromCacheAsync(tokens);
    }

    protected virtual async Task RemoveTokensFromCacheAsync(Func<CancellationToken, IAsyncEnumerable<OpenIddictTokenModel>> tokens)
    {
        // The revoke is already saved, so the caller's (or the request's) cancellation must not stop the cleanup.
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            try
            {
                await foreach (var token in tokens(cancellationTokenSource.Token))
                {
                    await Cache.RemoveAsync(token, cancellationTokenSource.Token);
                }
            }
            catch (Exception exception)
            {
                Logger.LogWarning(exception, "Could not remove the revoked tokens from the cache.");
            }
        }
    }
}
