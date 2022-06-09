using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OpenIddict.Tokens;

public class AbpOpenIddictTokenCache : AbpOpenIddictCacheBase<OpenIddictToken, OpenIddictTokenModel, IOpenIddictTokenStore<OpenIddictTokenModel>>,
    IOpenIddictTokenCache<OpenIddictTokenModel>,
    ITransientDependency
{
    public AbpOpenIddictTokenCache(
        IDistributedCache<OpenIddictTokenModel> cache,
        IDistributedCache<OpenIddictTokenModel[]> arrayCache,
        IOpenIddictTokenStore<OpenIddictTokenModel> store)
        : base(cache, arrayCache, store)
    {
    }

    public virtual async ValueTask AddAsync(OpenIddictTokenModel token, CancellationToken cancellationToken)
    {
        Check.NotNull(token, nameof(token));

        await RemoveAsync(token, cancellationToken);

        await Cache.SetAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(token, cancellationToken)}", token, token: cancellationToken);
        await Cache.SetAsync($"{nameof(FindByReferenceIdAsync)}_{await Store.GetReferenceIdAsync(token, cancellationToken)}", token, token: cancellationToken);
    }

    public virtual async IAsyncEnumerable<OpenIddictTokenModel> FindAsync(string subject, string client, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));

        var tokens = await ArrayCache.GetOrAddAsync($"{nameof(FindAsync)}_{subject}_{client}", async () =>
        {
            var tokens = new List<OpenIddictTokenModel>();
            await foreach (var token in Store.FindAsync(subject, client, cancellationToken))
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

    public virtual async IAsyncEnumerable<OpenIddictTokenModel> FindAsync(string subject, string client, string status, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));
        Check.NotNullOrEmpty(status, nameof(status));

        var tokens = await ArrayCache.GetOrAddAsync($"{nameof(FindAsync)}_{subject}_{client}_{status}", async () =>
        {
            var tokens = new List<OpenIddictTokenModel>();
            await foreach (var token in Store.FindAsync(subject, client, status, cancellationToken))
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

    public virtual async IAsyncEnumerable<OpenIddictTokenModel> FindAsync(string subject, string client, string status, string type, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));
        Check.NotNullOrEmpty(status, nameof(status));
        Check.NotNullOrEmpty(type, nameof(type));

        var tokens = await ArrayCache.GetOrAddAsync($"{nameof(FindAsync)}_{subject}_{client}_{status}_{type}", async () =>
        {
            var tokens = new List<OpenIddictTokenModel>();
            await foreach (var token in Store.FindAsync(subject, client, status, type, cancellationToken))
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

    public virtual async IAsyncEnumerable<OpenIddictTokenModel> FindByApplicationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

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
            $"{nameof(FindAsync)}_{await Store.GetSubjectAsync(token, cancellationToken)}_{await Store.GetApplicationIdAsync(token, cancellationToken)}",
            $"{nameof(FindAsync)}_{await Store.GetSubjectAsync(token, cancellationToken)}_{await Store.GetApplicationIdAsync(token, cancellationToken)}_{Store.GetStatusAsync(token, cancellationToken)}",
            $"{nameof(FindAsync)}_{await Store.GetSubjectAsync(token, cancellationToken)}_{await Store.GetApplicationIdAsync(token, cancellationToken)}_{Store.GetStatusAsync(token, cancellationToken)}_{Store.GetTypeAsync(token, cancellationToken)}",
            $"{nameof(FindByApplicationIdAsync)}_{await Store.GetApplicationIdAsync(token, cancellationToken)}",
            $"{nameof(FindByAuthorizationIdAsync)}_{await Store.GetAuthorizationIdAsync(token, cancellationToken)}",
            $"{nameof(FindBySubjectAsync)}_{await Store.GetSubjectAsync(token, cancellationToken)}"
        }, token: cancellationToken);

        await Cache.RemoveAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(token, cancellationToken)}", token: cancellationToken);
        await Cache.RemoveAsync($"{nameof(FindByReferenceIdAsync)}_{await Store.GetReferenceIdAsync(token, cancellationToken)}", token: cancellationToken);
    }
}
