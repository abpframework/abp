using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Tokens;

public class AbpOpenIddictTokenCache : AbpOpenIddictCacheBase<OpenIddictToken, OpenIddictTokenModel, IOpenIddictTokenStore<OpenIddictTokenModel>>,
    IOpenIddictTokenCache<OpenIddictTokenModel>,
    ITransientDependency
{
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public AbpOpenIddictTokenCache(
        IDistributedCache<OpenIddictTokenModel> cache,
        IDistributedCache<OpenIddictTokenModel[]> arrayCache,
        IOpenIddictTokenStore<OpenIddictTokenModel> store,
        IUnitOfWorkManager unitOfWorkManager)
        : base(cache, arrayCache, store)
    {
        UnitOfWorkManager = unitOfWorkManager;
    }

    public virtual async ValueTask AddAsync(OpenIddictTokenModel token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        await RemoveAsync(token, cancellationToken);

        if (IsInTransaction())
        {
            // Another request can cache the old committed value until the transaction is committed.
            // The request can be canceled after the commit, so its cancellation token isn't used.
            UnitOfWorkManager.Current.OnCompleted(async () =>
            {
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    await RemoveAsync(token, cancellationTokenSource.Token);
                }
            });
            return;
        }

        await Cache.SetAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(token, cancellationToken)}", token, token: cancellationToken);
        await Cache.SetAsync($"{nameof(FindByReferenceIdAsync)}_{await Store.GetReferenceIdAsync(token, cancellationToken)}", token, token: cancellationToken);
    }

    // Not cached: any filter can be omitted, so a changed token can't find all the cached lists that contain it.
    public virtual async IAsyncEnumerable<OpenIddictTokenModel> FindAsync(string subject, string client, string status, string type, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var token in Store.FindAsync(subject, client, status, type, cancellationToken))
        {
            yield return token;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictTokenModel> FindByApplicationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        if (IsInTransaction())
        {
            await foreach (var token in Store.FindByApplicationIdAsync(identifier, cancellationToken))
            {
                yield return token;
            }
            yield break;
        }

        var tokens = await ArrayCache.GetOrAddAsync($"{nameof(FindByApplicationIdAsync)}_{identifier}", async () =>
        {
            var tokens = new List<OpenIddictTokenModel>();
            await foreach (var token in Store.FindByApplicationIdAsync(identifier, cancellationToken))
            {
                tokens.Add(token);
                await AddAsync(token, cancellationToken);
            }
            return tokens.ToArray();
        }, token: cancellationToken);

        foreach (var token in tokens)
        {
            yield return token;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictTokenModel> FindByAuthorizationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        if (IsInTransaction())
        {
            await foreach (var token in Store.FindByAuthorizationIdAsync(identifier, cancellationToken))
            {
                yield return token;
            }
            yield break;
        }

        var tokens = await ArrayCache.GetOrAddAsync($"{nameof(FindByAuthorizationIdAsync)}_{identifier}", async () =>
        {
            var tokens = new List<OpenIddictTokenModel>();
            await foreach (var token in Store.FindByAuthorizationIdAsync(identifier, cancellationToken))
            {
                tokens.Add(token);
                await AddAsync(token, cancellationToken);
            }
            return tokens.ToArray();
        }, token: cancellationToken);

        foreach (var token in tokens)
        {
            yield return token;
        }
    }

    public virtual async ValueTask<OpenIddictTokenModel> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        if (IsInTransaction())
        {
            return await Store.FindByIdAsync(identifier, cancellationToken);
        }

        return await Cache.GetOrAddAsync($"{nameof(FindByIdAsync)}_{identifier}",  async () =>
        {
            var token = await Store.FindByIdAsync(identifier, cancellationToken);
            if (token != null)
            {
                await AddAsync(token, cancellationToken);
            }
            return token;
        }, token: cancellationToken);
    }

    public virtual async ValueTask<OpenIddictTokenModel> FindByReferenceIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        if (IsInTransaction())
        {
            return await Store.FindByReferenceIdAsync(identifier, cancellationToken);
        }

        return await Cache.GetOrAddAsync($"{nameof(FindByReferenceIdAsync)}_{identifier}",  async () =>
        {
            var token = await Store.FindByReferenceIdAsync(identifier, cancellationToken);
            if (token != null)
            {
                await AddAsync(token, cancellationToken);
            }
            return token;
        }, token: cancellationToken);
    }

    public virtual async IAsyncEnumerable<OpenIddictTokenModel> FindBySubjectAsync(string subject, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));

        if (IsInTransaction())
        {
            await foreach (var token in Store.FindBySubjectAsync(subject, cancellationToken))
            {
                yield return token;
            }
            yield break;
        }

        var tokens = await ArrayCache.GetOrAddAsync($"{nameof(FindBySubjectAsync)}_{subject}", async () =>
        {
            var tokens = new List<OpenIddictTokenModel>();
            await foreach (var token in Store.FindBySubjectAsync(subject, cancellationToken))
            {
                tokens.Add(token);
                await AddAsync(token, cancellationToken);
            }
            return tokens.ToArray();
        }, token: cancellationToken);

        foreach (var token in tokens)
        {
            yield return token;
        }
    }

    public virtual async ValueTask RemoveAsync(OpenIddictTokenModel token, CancellationToken cancellationToken)
    {
        await ArrayCache.RemoveManyAsync(new[]
        {
            $"{nameof(FindByApplicationIdAsync)}_{await Store.GetApplicationIdAsync(token, cancellationToken)}",
            $"{nameof(FindByAuthorizationIdAsync)}_{await Store.GetAuthorizationIdAsync(token, cancellationToken)}",
            $"{nameof(FindBySubjectAsync)}_{await Store.GetSubjectAsync(token, cancellationToken)}"
        }, token: cancellationToken);

        await Cache.RemoveAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(token, cancellationToken)}", token: cancellationToken);
        await Cache.RemoveAsync($"{nameof(FindByReferenceIdAsync)}_{await Store.GetReferenceIdAsync(token, cancellationToken)}", token: cancellationToken);
    }

    // A value read inside a transaction may not be committed yet, so it's read from the store and not cached.
    protected virtual bool IsInTransaction()
    {
        return UnitOfWorkManager.Current?.Options.IsTransactional == true;
    }
}
