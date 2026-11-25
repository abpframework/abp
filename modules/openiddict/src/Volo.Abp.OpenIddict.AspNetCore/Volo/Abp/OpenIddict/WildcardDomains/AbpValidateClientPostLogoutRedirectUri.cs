using System.Threading.Tasks;
using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public class AbpValidateClientPostLogoutRedirectUri : AbpOpenIddictWildcardDomainBase<AbpValidateClientPostLogoutRedirectUri, OpenIddictServerHandlers.Session.ValidateClientPostLogoutRedirectUri, OpenIddictServerEvents.ValidateEndSessionRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateEndSessionRequestContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireDegradedModeDisabled>()
            .AddFilter<OpenIddictServerHandlerFilters.RequirePostLogoutRedirectUriParameter>()
            .UseScopedHandler<AbpValidateClientPostLogoutRedirectUri>()
            .SetOrder(OpenIddictServerHandlers.Session.ValidateClientPostLogoutRedirectUri.Descriptor.Order)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    public AbpValidateClientPostLogoutRedirectUri(
        IOpenIddictApplicationManager applicationManager,
        AbpOpenIddictWildcardDomainValidator wildcardDomainValidator)
        : base(new OpenIddictServerHandlers.Session.ValidateClientPostLogoutRedirectUri(applicationManager), wildcardDomainValidator)
    {
    }

    public async override ValueTask HandleAsync(OpenIddictServerEvents.ValidateEndSessionRequestContext context)
    {
        Check.NotNull(context, nameof(context));
        Check.NotNullOrEmpty(context.PostLogoutRedirectUri, nameof(context.PostLogoutRedirectUri));

        if ((!string.IsNullOrEmpty(context.ClientId) &&
                await WildcardDomainValidator.IsValidAsync(context.PostLogoutRedirectUri, context.ClientId, postLogout: true)) ||
            (string.IsNullOrEmpty(context.ClientId) && await WildcardDomainValidator.IsValidPostLogoutAsync(
                context.PostLogoutRedirectUri, context.IdentityTokenHintPrincipal, context.Options.IgnoreEndpointPermissions)))
        {
            return;
        }

        await OriginalHandler.HandleAsync(context);
    }
}
