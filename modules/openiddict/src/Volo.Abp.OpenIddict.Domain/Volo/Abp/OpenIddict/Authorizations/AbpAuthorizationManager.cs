using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Authorizations;

public class AbpAuthorizationManager : OpenIddictAuthorizationManager<OpenIddictAuthorizationModel>
{
    protected AbpOpenIddictIdentifierConverter IdentifierConverter { get; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected IOpenIddictTokenCache<OpenIddictTokenModel> TokenCache { get; }

    protected IOpenIddictTokenStore<OpenIddictTokenModel> TokenStore { get; }

    public AbpAuthorizationManager(
        [NotNull] [ItemNotNull] IOpenIddictAuthorizationCache<OpenIddictAuthorizationModel> cache,
        [NotNull] [ItemNotNull] ILogger<OpenIddictAuthorizationManager<OpenIddictAuthorizationModel>> logger,
        [NotNull] [ItemNotNull] IOptionsMonitor<OpenIddictCoreOptions> options,
        [NotNull] IOpenIddictAuthorizationStore<OpenIddictAuthorizationModel> resolver,
        AbpOpenIddictIdentifierConverter identifierConverter,
        IUnitOfWorkManager unitOfWorkManager,
        IOpenIddictTokenCache<OpenIddictTokenModel> tokenCache,
        IOpenIddictTokenStore<OpenIddictTokenModel> tokenStore)
        : base(cache, logger, options, resolver)
    {
        IdentifierConverter = identifierConverter;
        UnitOfWorkManager = unitOfWorkManager;
        TokenCache = tokenCache;
        TokenStore = tokenStore;
    }

    public async override ValueTask UpdateAsync(OpenIddictAuthorizationModel authorization, CancellationToken cancellationToken = default)
    {
        if (!Options.CurrentValue.DisableEntityCaching)
        {
            var entity = await Store.FindByIdAsync(IdentifierConverter.ToString(authorization.Id), cancellationToken);
            if (entity != null)
            {
                await Cache.RemoveAsync(entity, cancellationToken);
            }
        }

        await base.UpdateAsync(authorization, cancellationToken);
    }

    public async override ValueTask DeleteAsync(OpenIddictAuthorizationModel authorization, CancellationToken cancellationToken = default)
    {
        var tokens = new List<OpenIddictTokenModel>();
        if (!Options.CurrentValue.DisableEntityCaching)
        {
            await foreach (var token in TokenStore.FindByAuthorizationIdAsync(IdentifierConverter.ToString(authorization.Id), cancellationToken))
            {
                tokens.Add(token);
            }
        }

        await base.DeleteAsync(authorization, cancellationToken);

        // The store deletes the tokens in its own unit of work, so they are already gone from the database here.
        await RemoveDeletedTokensFromCacheAsync(tokens);
    }

    public async override ValueTask<long> RevokeAsync(string subject, string client, string status, string type, CancellationToken cancellationToken = default)
    {
        var count = await base.RevokeAsync(subject, client, status, type, cancellationToken);

        // The status filter is omitted because the revoked authorizations no longer match it.
        await RemoveFromCacheAsync(ct => Store.FindAsync(subject, client, null, type, ImmutableArray<string>.Empty, ct));

        return count;
    }

    public async override ValueTask<long> RevokeByApplicationIdAsync(string identifier, CancellationToken cancellationToken = default)
    {
        var count = await base.RevokeByApplicationIdAsync(identifier, cancellationToken);
        await RemoveFromCacheAsync(ct => Store.FindByApplicationIdAsync(identifier, ct));
        return count;
    }

    public async override ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        var count = await base.RevokeBySubjectAsync(subject, cancellationToken);
        await RemoveFromCacheAsync(ct => Store.FindBySubjectAsync(subject, ct));
        return count;
    }

    protected virtual async Task RemoveFromCacheAsync(Func<CancellationToken, IAsyncEnumerable<OpenIddictAuthorizationModel>> authorizations)
    {
        if (Options.CurrentValue.DisableEntityCaching)
        {
            return;
        }

        // Inside an outer transactional unit of work the revoke is not committed yet, so the cache is cleared after the unit of work is completed.
        var unitOfWork = UnitOfWorkManager.Current;
        if (unitOfWork != null && unitOfWork.Options.IsTransactional)
        {
            unitOfWork.OnCompleted(() => RemoveAuthorizationsFromCacheAsync(authorizations));
            return;
        }

        await RemoveAuthorizationsFromCacheAsync(authorizations);
    }

    protected virtual async Task RemoveAuthorizationsFromCacheAsync(Func<CancellationToken, IAsyncEnumerable<OpenIddictAuthorizationModel>> authorizations)
    {
        // The revoke is already saved, so the caller's (or the request's) cancellation must not stop the cleanup.
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            try
            {
                await foreach (var authorization in authorizations(cancellationTokenSource.Token))
                {
                    await Cache.RemoveAsync(authorization, cancellationTokenSource.Token);
                }
            }
            catch (Exception exception)
            {
                Logger.LogWarning(exception, "Could not remove the revoked authorizations from the cache.");
            }
        }
    }

    protected virtual async Task RemoveDeletedTokensFromCacheAsync(List<OpenIddictTokenModel> tokens)
    {
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            try
            {
                foreach (var token in tokens)
                {
                    await TokenCache.RemoveAsync(token, cancellationTokenSource.Token);
                }
            }
            catch (Exception exception)
            {
                Logger.LogWarning(exception, "Could not remove the deleted tokens from the cache.");
            }
        }
    }
}
