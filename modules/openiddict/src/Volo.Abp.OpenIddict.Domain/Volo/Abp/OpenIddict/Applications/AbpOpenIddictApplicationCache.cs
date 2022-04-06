using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OpenIddict.Applications;

public class AbpOpenIddictApplicationCache : AbpOpenIddictCacheBase<OpenIddictApplication, OpenIddictApplicationModel, IOpenIddictApplicationStore<OpenIddictApplicationModel>>,
    IOpenIddictApplicationCache<OpenIddictApplicationModel>,
    ITransientDependency
{
    public AbpOpenIddictApplicationCache(
        IDistributedCache<OpenIddictApplicationModel> cache,
        IDistributedCache<OpenIddictApplicationModel[]> arrayCache,
        IOpenIddictApplicationStore<OpenIddictApplicationModel> store)
        : base(cache, arrayCache, store)
    {
    }

    public virtual async ValueTask AddAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        await RemoveAsync(application, cancellationToken);

        await Cache.SetManyAsync(new List<KeyValuePair<string, OpenIddictApplicationModel>>
        {
            new KeyValuePair<string, OpenIddictApplicationModel>($"{nameof(FindByClientIdAsync)}_{await Store.GetClientIdAsync(application, cancellationToken)}", application),
            new KeyValuePair<string, OpenIddictApplicationModel>($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(application, cancellationToken)}", application)
        }, token: cancellationToken);
    }

    public virtual async ValueTask<OpenIddictApplicationModel> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return await Cache.GetOrAddAsync($"{nameof(FindByClientIdAsync)}_{identifier}", async () =>
        {
           var application = await Store.FindByClientIdAsync(identifier, cancellationToken);
           if (application != null)
           {
               await AddAsync(application, cancellationToken);
           }
           return application;
        }, token: cancellationToken);
    }

    public virtual async ValueTask<OpenIddictApplicationModel> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(identifier, nameof(identifier));

        return await Cache.GetOrAddAsync($"{nameof(FindByIdAsync)}_{identifier}", async () =>
        {
            var application = await Store.FindByIdAsync(identifier, cancellationToken);
            if (application != null)
            {
                await AddAsync(application, cancellationToken);
            }
            return application;
        }, token: cancellationToken);
    }

    public virtual async IAsyncEnumerable<OpenIddictApplicationModel> FindByPostLogoutRedirectUriAsync(string address, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(address, nameof(address));

        var applications = await ArrayCache.GetOrAddAsync($"{nameof(FindByPostLogoutRedirectUriAsync)}_{address}", async () =>
        {
            var applications = new List<OpenIddictApplicationModel>();
            await foreach (var application in Store.FindByPostLogoutRedirectUriAsync(address, cancellationToken))
            {
                applications.Add(application);
                await AddAsync(application, cancellationToken);
            }
            return applications.ToArray();

        }, token: cancellationToken);

        foreach (var application in applications)
        {
            yield return application;
        }
    }

    public virtual async IAsyncEnumerable<OpenIddictApplicationModel> FindByRedirectUriAsync(string address, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        Check.NotNullOrEmpty(address, nameof(address));

        var applications = await ArrayCache.GetOrAddAsync($"{nameof(FindByRedirectUriAsync)}_{address}", async () =>
        {
            var applications = new List<OpenIddictApplicationModel>();
            await foreach (var application in Store.FindByRedirectUriAsync(address, cancellationToken))
            {
                applications.Add(application);
                await AddAsync(application, cancellationToken);
            }
            return applications.ToArray();

        }, token: cancellationToken);

        foreach (var application in applications)
        {
            yield return application;
        }
    }

    public virtual async ValueTask RemoveAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken)
    {
        Check.NotNull(application, nameof(application));

        await Cache.RemoveAsync($"{nameof(FindByClientIdAsync)}_{await Store.GetClientIdAsync(application, cancellationToken)}", token: cancellationToken);
        await Cache.RemoveAsync($"{nameof(FindByIdAsync)}_{await Store.GetIdAsync(application, cancellationToken)}", token: cancellationToken);

        var redirectUris = await Store.GetRedirectUrisAsync(application, cancellationToken);
        await ArrayCache.RemoveManyAsync(redirectUris.Select(address => $"{nameof(FindByRedirectUriAsync)}_{address}").ToArray(), token: cancellationToken);

        var postLogoutRedirectUris = await Store.GetPostLogoutRedirectUrisAsync(application, cancellationToken);
        await ArrayCache.RemoveManyAsync(postLogoutRedirectUris.Select(address => $"{nameof(FindByPostLogoutRedirectUriAsync)}_{address}").ToArray(), token: cancellationToken);
    }
}
