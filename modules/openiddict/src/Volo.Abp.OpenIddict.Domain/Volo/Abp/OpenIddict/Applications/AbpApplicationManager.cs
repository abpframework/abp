using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.OpenIddict.Applications;

public class AbpApplicationManager : OpenIddictApplicationManager<OpenIddictApplicationModel>, IAbpApplicationManager
{
    protected AbpOpenIddictIdentifierConverter IdentifierConverter { get; }
    protected IDistributedEventBus DistributedEventBus { get; }

    public AbpApplicationManager(
        [NotNull] IOpenIddictApplicationCache<OpenIddictApplicationModel> cache,
        [NotNull] ILogger<AbpApplicationManager> logger,
        [NotNull] IOptionsMonitor<OpenIddictCoreOptions> options,
        [NotNull] IOpenIddictApplicationStore<OpenIddictApplicationModel> resolver,
        AbpOpenIddictIdentifierConverter identifierConverter,
        IDistributedEventBus distributedEventBus)
        : base(cache, logger, options, resolver)
    {
        IdentifierConverter = identifierConverter;
        DistributedEventBus = distributedEventBus;
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
