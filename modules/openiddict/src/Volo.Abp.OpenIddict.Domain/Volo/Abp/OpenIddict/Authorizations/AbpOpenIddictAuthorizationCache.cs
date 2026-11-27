using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Authorizations;

public class AbpOpenIddictAuthorizationCache : AbpOpenIddictCacheBase<OpenIddictAuthorization, OpenIddictAuthorizationModel, IOpenIddictAuthorizationStore<OpenIddictAuthorizationModel>>,
    IOpenIddictAuthorizationCache<OpenIddictAuthorizationModel>,
    ITransientDependency
{
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public AbpOpenIddictAuthorizationCache(
        IDistributedCache<OpenIddictAuthorizationModel> cache,
        IDistributedCache<OpenIddictAuthorizationModel[]> arrayCache,
        IOpenIddictAuthorizationStore<OpenIddictAuthorizationModel> store,
        IUnitOfWorkManager unitOfWorkManager)
        : base(cache, arrayCache, store)
    {
        UnitOfWorkManager = unitOfWorkManager;
    }

    public virtual async ValueTask AddAsync(OpenIddictAuthorizationModel authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        await RemoveAsync(authorization, cancellationToken);

        if (IsInTransaction())
        {
            // Another request can cache the old committed value until the transaction is committed.
            // The request can be canceled after the commit, so its cancellation token isn't used.
            UnitOfWorkManager.Current.OnCompleted(async () =>
            {
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    await RemoveAsync(authorization, cancellationTokenSource.Token);
                }
            });
            return;
        }

        await Cache.SetAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(authorization, cancellationToken)}", authorization, token: cancellationToken);
    }

    public virtual async IAsyncEnumerable<OpenIddictAuthorizationModel> FindAsync(string subject, string client, string status, string type, ImmutableArray<string>? scopes, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        // Note: this method is only partially cached.
        await foreach (var authorization in Store.FindAsync(subject, client, status, type, scopes, cancellationToken))
        {
            await AddAsync(authorization, cancellationToken);
            yield return authorization;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictAuthorizationModel> FindByApplicationIdAsync(string applicationId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(applicationId, nameof(applicationId));

        if (IsInTransaction())
        {
            await foreach (var authorization in Store.FindByApplicationIdAsync(applicationId, cancellationToken))
            {
                yield return authorization;
            }
            yield break;
        }

        var authorizations = await ArrayCache.GetOrAddAsync($"{nameof(FindByApplicationIdAsync)}_{applicationId}", async () =>
        {
            var applications = new List<OpenIddictAuthorizationModel>();
            await foreach (var authorization in Store.FindByApplicationIdAsync(applicationId, cancellationToken))
            {
                applications.Add(authorization);
                await AddAsync(authorization, cancellationToken);
            }
            return applications.ToArray();
        }, token: cancellationToken);

        foreach (var authorization in authorizations)
        {
            yield return authorization;
        }
    }

    public virtual async ValueTask<OpenIddictAuthorizationModel> FindByIdAsync(string id, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(id, nameof(id));

        if (IsInTransaction())
        {
            return await Store.FindByIdAsync(id, cancellationToken);
        }

        return await Cache.GetOrAddAsync($"{nameof(FindByIdAsync)}_{id}",
            async () => await Store.FindByIdAsync(id, cancellationToken), token: cancellationToken);
    }

    public virtual async IAsyncEnumerable<OpenIddictAuthorizationModel> FindBySubjectAsync(string subject, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));

        if (IsInTransaction())
        {
            await foreach (var authorization in Store.FindBySubjectAsync(subject, cancellationToken))
            {
                yield return authorization;
            }
            yield break;
        }

        var authorizations = await ArrayCache.GetOrAddAsync($"{nameof(FindBySubjectAsync)}_{subject}", async () =>
        {
            var applications = new List<OpenIddictAuthorizationModel>();
            await foreach (var authorization in Store.FindBySubjectAsync(subject, cancellationToken))
            {
                applications.Add(authorization);
                await AddAsync(authorization, cancellationToken);
            }
            return applications.ToArray();
        }, token: cancellationToken);

        foreach (var authorization in authorizations)
        {
            yield return authorization;
        }
    }

    public virtual async ValueTask RemoveAsync(OpenIddictAuthorizationModel authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        await ArrayCache.RemoveManyAsync(new[]
        {
            $"{nameof(FindAsync)}_{await Store.GetSubjectAsync(authorization, cancellationToken)}_{await Store.GetApplicationIdAsync(authorization, cancellationToken)}_{await Store.GetStatusAsync(authorization, cancellationToken)}_{await Store.GetTypeAsync(authorization, cancellationToken)}",
            $"{nameof(FindByApplicationIdAsync)}_{await Store.GetApplicationIdAsync(authorization, cancellationToken)}",
            $"{nameof(FindBySubjectAsync)}_{await Store.GetSubjectAsync(authorization, cancellationToken)}"
        }, token: cancellationToken);

        await Cache.RemoveAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(authorization, cancellationToken)}", token: cancellationToken);
    }

    // A value read inside a transaction may not be committed yet, so it's read from the store and not cached.
    protected virtual bool IsInTransaction()
    {
        return UnitOfWorkManager.Current?.Options.IsTransactional == true;
    }
}
