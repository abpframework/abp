using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OpenIddict.Authorizations;

public class AbpOpenIddictAuthorizationCache : AbpOpenIddictCacheBase<OpenIddictAuthorization, OpenIddictAuthorizationModel, IOpenIddictAuthorizationStore<OpenIddictAuthorizationModel>>,
    IOpenIddictAuthorizationCache<OpenIddictAuthorizationModel>,
    ITransientDependency
{
    public AbpOpenIddictAuthorizationCache(
        IDistributedCache<OpenIddictAuthorizationModel> cache,
        IDistributedCache<OpenIddictAuthorizationModel[]> arrayCache,
        IOpenIddictAuthorizationStore<OpenIddictAuthorizationModel> store)
        : base(cache, arrayCache, store)
    {
    }

    public async ValueTask AddAsync(OpenIddictAuthorizationModel authorization, CancellationToken cancellationToken)
    {
        Check.NotNull(authorization, nameof(authorization));

        await RemoveAsync(authorization, cancellationToken);

        await Cache.SetAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(authorization, cancellationToken)}", authorization, token: cancellationToken);
    }

    public async IAsyncEnumerable<OpenIddictAuthorizationModel> FindAsync(string subject, string client, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));

        var authorizations = await ArrayCache.GetOrAddAsync($"{nameof(FindAsync)}_{subject}_{client}", async () =>
        {
            var applications = new List<OpenIddictAuthorizationModel>();
            await foreach (var authorization in Store.FindAsync(subject, client, cancellationToken))
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

    public async IAsyncEnumerable<OpenIddictAuthorizationModel> FindAsync(string subject, string client, string status, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));
        Check.NotNullOrEmpty(status, nameof(status));

        var authorizations = await ArrayCache.GetOrAddAsync($"{nameof(FindAsync)}_{subject}_{client}_{status}", async () =>
        {
            var applications = new List<OpenIddictAuthorizationModel>();
            await foreach (var authorization in Store.FindAsync(subject, client, status, cancellationToken))
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

    public async IAsyncEnumerable<OpenIddictAuthorizationModel> FindAsync(string subject, string client, string status, string type, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));
        Check.NotNullOrEmpty(status, nameof(status));
        Check.NotNullOrEmpty(type, nameof(type));

        var authorizations = await ArrayCache.GetOrAddAsync($"{nameof(FindAsync)}_{subject}_{client}_{status}_{type}", async () =>
        {
            var applications = new List<OpenIddictAuthorizationModel>();
            await foreach (var authorization in Store.FindAsync(subject, client, status, type, cancellationToken))
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

    public async IAsyncEnumerable<OpenIddictAuthorizationModel> FindAsync(string subject, string client, string status, string type, ImmutableArray<string> scopes, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));
        Check.NotNullOrEmpty(client, nameof(client));
        Check.NotNullOrEmpty(status, nameof(status));
        Check.NotNullOrEmpty(type, nameof(type));

        // Note: this method is only partially cached.
        await foreach (var authorization in Store.FindAsync(subject, client, status, type, scopes, cancellationToken))
        {
            await AddAsync(authorization, cancellationToken);
            yield return authorization;
        }
    }

    public async IAsyncEnumerable<OpenIddictAuthorizationModel> FindByApplicationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        var authorizations = await ArrayCache.GetOrAddAsync($"{nameof(FindByApplicationIdAsync)}_{identifier}", async () =>
        {
            var applications = new List<OpenIddictAuthorizationModel>();
            await foreach (var authorization in Store.FindByApplicationIdAsync(identifier, cancellationToken))
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

    public async ValueTask<OpenIddictAuthorizationModel> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return await Cache.GetOrAddAsync($"{nameof(FindByIdAsync)}_{identifier}",
            async () => await Store.FindByIdAsync(identifier, cancellationToken), token: cancellationToken);
    }

    public async IAsyncEnumerable<OpenIddictAuthorizationModel> FindBySubjectAsync(string subject, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(subject, nameof(subject));

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
            $"{nameof(FindAsync)}_{await Store.GetSubjectAsync(authorization, cancellationToken)}_{await Store.GetApplicationIdAsync(authorization, cancellationToken)}",
            $"{nameof(FindAsync)}_{await Store.GetSubjectAsync(authorization, cancellationToken)}_{await Store.GetApplicationIdAsync(authorization, cancellationToken)}_{await Store.GetStatusAsync(authorization, cancellationToken)}",
            $"{nameof(FindAsync)}_{await Store.GetSubjectAsync(authorization, cancellationToken)}_{await Store.GetApplicationIdAsync(authorization, cancellationToken)}_{await Store.GetStatusAsync(authorization, cancellationToken)}_{await Store.GetTypeAsync(authorization, cancellationToken)}",
            $"{nameof(FindByApplicationIdAsync)}_{await Store.GetApplicationIdAsync(authorization, cancellationToken)}",
            $"{nameof(FindBySubjectAsync)}_{await Store.GetSubjectAsync(authorization, cancellationToken)}"
        }, token: cancellationToken);

        await Cache.RemoveAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(authorization, cancellationToken)}", token: cancellationToken);
    }
}
