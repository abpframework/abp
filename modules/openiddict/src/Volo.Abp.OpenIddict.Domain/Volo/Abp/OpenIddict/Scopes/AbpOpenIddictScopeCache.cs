using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OpenIddict.Scopes;

public class AbpOpenIddictScopeCacheAbpOpenIddictAuthorizationCache : AbpOpenIddictCacheBase<OpenIddictScope, OpenIddictScopeModel, IOpenIddictScopeStore<OpenIddictScopeModel>>,
    IOpenIddictScopeCache<OpenIddictScopeModel>,
    ITransientDependency
{
    public AbpOpenIddictScopeCacheAbpOpenIddictAuthorizationCache(
        IDistributedCache<OpenIddictScopeModel> cache,
        IDistributedCache<OpenIddictScopeModel[]> arrayCache,
        IOpenIddictScopeStore<OpenIddictScopeModel> store)
        : base(cache, arrayCache, store)
    {
    }

    public virtual async ValueTask AddAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        await RemoveAsync(scope, cancellationToken);

        await Cache.SetAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(scope, cancellationToken)}", scope, token: cancellationToken);
        await Cache.SetAsync($"{nameof(FindByNameAsync)}_{await Store.GetNameAsync(scope, cancellationToken)}", scope, token: cancellationToken);
    }

    public virtual async ValueTask<OpenIddictScopeModel> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return await Cache.GetOrAddAsync($"{nameof(FindByIdAsync)}_{identifier}",  async () =>
        {
            var scope = await Store.FindByIdAsync(identifier, cancellationToken);
            if (scope != null)
            {
                await AddAsync(scope, cancellationToken);
            }
            return scope;
        }, token: cancellationToken);
    }

    public virtual async ValueTask<OpenIddictScopeModel> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(name, nameof(name));

        return await Cache.GetOrAddAsync($"{nameof(FindByNameAsync)}_{name}",  async () =>
        {
            var scope = await Store.FindByNameAsync(name, cancellationToken);
            if (scope != null)
            {
                await AddAsync(scope, cancellationToken);
            }
            return scope;
        }, token: cancellationToken);
    }

    public virtual async IAsyncEnumerable<OpenIddictScopeModel> FindByNamesAsync(ImmutableArray<string> names, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(names, nameof(names));

        // Note: this method is only partially cached.
        await foreach (var scope in Store.FindByNamesAsync(names, cancellationToken))
        {
            await AddAsync(scope, cancellationToken);
            yield return scope;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictScopeModel> FindByResourceAsync(string resource, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(resource, nameof(resource));

        var scopes = await ArrayCache.GetOrAddAsync($"{nameof(FindByResourceAsync)}_{resource}", async () =>
        {
            var scopes = new List<OpenIddictScopeModel>();
            await foreach (var scope in Store.FindByResourceAsync(resource, cancellationToken))
            {
                scopes.Add(scope);
                await AddAsync(scope, cancellationToken);
            }
            return scopes.ToArray();
        }, token: cancellationToken);

        foreach (var scope in scopes)
        {
            yield return scope;
        }
    }

    public virtual async ValueTask RemoveAsync(OpenIddictScopeModel scope, CancellationToken cancellationToken)
    {
        Check.NotNull(scope, nameof(scope));

        var resources = new List<string>();
        foreach (var resource in await Store.GetResourcesAsync(scope, cancellationToken))
        {
            resources.Add($"{nameof(FindByResourceAsync)}_{resource}");
        }
        await ArrayCache.RemoveManyAsync(resources.ToArray(), token: cancellationToken);

        await Cache.RemoveAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(scope, cancellationToken)}", token: cancellationToken);
        await Cache.RemoveAsync($"{nameof(FindByNameAsync)}_{await Store.GetNameAsync(scope, cancellationToken)}", token: cancellationToken);
    }
}
