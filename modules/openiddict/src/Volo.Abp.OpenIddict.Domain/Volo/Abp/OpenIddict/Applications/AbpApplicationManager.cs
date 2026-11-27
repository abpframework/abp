using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.Applications;

public class AbpApplicationManager : OpenIddictApplicationManager<OpenIddictApplicationModel>, IAbpApplicationManager
{
    protected AbpOpenIddictIdentifierConverter IdentifierConverter { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected IOpenIddictTokenCache<OpenIddictTokenModel> TokenCache { get; }
    protected IOpenIddictTokenStore<OpenIddictTokenModel> TokenStore { get; }

    public AbpApplicationManager(
        [NotNull] IOpenIddictApplicationCache<OpenIddictApplicationModel> cache,
        [NotNull] ILogger<AbpApplicationManager> logger,
        [NotNull] IOptionsMonitor<OpenIddictCoreOptions> options,
        [NotNull] IOpenIddictApplicationStore<OpenIddictApplicationModel> resolver,
        AbpOpenIddictIdentifierConverter identifierConverter,
        IDistributedEventBus distributedEventBus,
        IOpenIddictTokenCache<OpenIddictTokenModel> tokenCache,
        IOpenIddictTokenStore<OpenIddictTokenModel> tokenStore)
        : base(cache, logger, options, resolver)
    {
        IdentifierConverter = identifierConverter;
        DistributedEventBus = distributedEventBus;
        TokenCache = tokenCache;
        TokenStore = tokenStore;
    }

    public override async ValueTask DeleteAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken = default)
    {
        var tokens = new List<OpenIddictTokenModel>();
        if (!Options.CurrentValue.DisableEntityCaching)
        {
            await foreach (var token in TokenStore.FindByApplicationIdAsync(IdentifierConverter.ToString(application.Id), cancellationToken))
            {
                tokens.Add(token);
            }
        }

        await base.DeleteAsync(application, cancellationToken);

        // The store deletes the tokens in its own unit of work, so they are already gone from the database here.
        await RemoveDeletedTokensFromCacheAsync(tokens);
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

    public override async ValueTask UpdateAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken = default)
    {
        var entity = await Store.FindByIdAsync(IdentifierConverter.ToString(application.Id), cancellationToken);
        var oldClientId = entity?.ClientId;

        if (!Options.CurrentValue.DisableEntityCaching)
        {
            if (entity != null)
            {
                await Cache.RemoveAsync(entity, cancellationToken);
            }
        }

        await base.UpdateAsync(application, cancellationToken);

        if (oldClientId != null &&
            application.ClientId != null &&
            oldClientId != application.ClientId)
        {
            await DistributedEventBus.PublishAsync(new OpenIddictApplicationClientIdChangedEto
            {
                Id = application.Id,
                OldClientId = oldClientId,
                ClientId = application.ClientId
            });
        }
    }

    public override async ValueTask PopulateAsync(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationModel application, CancellationToken cancellationToken = default)
    {
        await base.PopulateAsync(descriptor, application, cancellationToken);

        if (descriptor is AbpApplicationDescriptor model)
        {

            if (!application.FrontChannelLogoutUri.IsNullOrWhiteSpace())
            {
                if (!Uri.TryCreate(application.FrontChannelLogoutUri, UriKind.Absolute, out var uri) || IsImplicitFileUri(uri))
                {
                    throw new ArgumentException(OpenIddictResources.GetResourceString("ID0214"));
                }

                model.FrontChannelLogoutUri = uri;
            }

            model.ClientUri = application.ClientUri;
            model.LogoUri = application.LogoUri;
        }
    }

    public override async ValueTask PopulateAsync(OpenIddictApplicationModel application, OpenIddictApplicationDescriptor descriptor, CancellationToken cancellationToken = default)
    {
        await base.PopulateAsync(application, descriptor, cancellationToken);

        if (descriptor is AbpApplicationDescriptor model)
        {
            application.FrontChannelLogoutUri = model.FrontChannelLogoutUri?.OriginalString;
            application.ClientUri = model.ClientUri;
            application.LogoUri = model.LogoUri;
        }
    }

    public virtual async ValueTask<string> GetFrontChannelLogoutUriAsync(object application, CancellationToken cancellationToken = default)
    {
        Check.NotNull(application, nameof(application));
        Check.AssignableTo<OpenIddictApplicationModel>(application.GetType(), nameof(application));

        return await Store.As<IAbpOpenIdApplicationStore>().GetFrontChannelLogoutUriAsync(application.As<OpenIddictApplicationModel>(), cancellationToken);
    }

    public virtual async ValueTask<string> GetClientUriAsync(object application, CancellationToken cancellationToken = default)
    {
        Check.NotNull(application, nameof(application));
        Check.AssignableTo<OpenIddictApplicationModel>(application.GetType(), nameof(application));

        return await Store.As<IAbpOpenIdApplicationStore>().GetClientUriAsync(application.As<OpenIddictApplicationModel>(), cancellationToken);
    }

    public virtual async ValueTask<string> GetLogoUriAsync(object application, CancellationToken cancellationToken = default)
    {
        Check.NotNull(application, nameof(application));
        Check.AssignableTo<OpenIddictApplicationModel>(application.GetType(), nameof(application));

        return await Store.As<IAbpOpenIdApplicationStore>().GetLogoUriAsync(application.As<OpenIddictApplicationModel>(), cancellationToken);
    }

    protected virtual bool IsImplicitFileUri(Uri uri)
    {
        Check.NotNull(uri, nameof(uri));

        return uri.IsAbsoluteUri && uri.IsFile && !uri.OriginalString.StartsWith(uri.Scheme, StringComparison.OrdinalIgnoreCase);
    }
}
