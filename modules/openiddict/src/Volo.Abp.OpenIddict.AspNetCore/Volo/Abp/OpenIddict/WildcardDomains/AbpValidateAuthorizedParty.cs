using System.Threading.Tasks;
using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public class AbpValidateAuthorizedParty : AbpOpenIddictWildcardDomainBase<AbpValidateAuthorizedParty, OpenIddictServerHandlers.Session.ValidateAuthorizedParty, OpenIddictServerEvents.ValidateEndSessionRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateEndSessionRequestContext>()
            .UseScopedHandler<AbpValidateAuthorizedParty>()
            .SetOrder(OpenIddictServerHandlers.Session.ValidateEndpointPermissions.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    public AbpValidateAuthorizedParty(
        IOpenIddictApplicationManager applicationManager,
        AbpOpenIddictWildcardDomainValidator wildcardDomainValidator)
        : base(new OpenIddictServerHandlers.Session.ValidateAuthorizedParty(applicationManager), wildcardDomainValidator)
    {
    }

    public async override ValueTask HandleAsync(OpenIddictServerEvents.ValidateEndSessionRequestContext context)
    {
        Check.NotNull(context, nameof(context));

        if (string.IsNullOrEmpty(context.ClientId) && context.IdentityTokenHintPrincipal != null &&
            !string.IsNullOrEmpty(context.PostLogoutRedirectUri) &&
            await WildcardDomainValidator.IsValidPostLogoutAsync(context.PostLogoutRedirectUri,
                context.IdentityTokenHintPrincipal, context.Options.IgnoreEndpointPermissions))
        {
            return;
        }

        await OriginalHandler.HandleAsync(context);
    }
}
