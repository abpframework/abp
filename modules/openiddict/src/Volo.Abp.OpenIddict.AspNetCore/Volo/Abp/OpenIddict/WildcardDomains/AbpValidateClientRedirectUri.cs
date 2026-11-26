using System.Threading.Tasks;
using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public class AbpValidateClientRedirectUri : AbpOpenIddictWildcardDomainBase<AbpValidateClientRedirectUri, OpenIddictServerHandlers.Authentication.ValidateClientRedirectUri, OpenIddictServerEvents.ValidateAuthorizationRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateAuthorizationRequestContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireDegradedModeDisabled>()
            .UseScopedHandler<AbpValidateClientRedirectUri>()
            .SetOrder(OpenIddictServerHandlers.Authentication.ValidateResponseType.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    public AbpValidateClientRedirectUri(
        IOpenIddictApplicationManager applicationManager,
        AbpOpenIddictWildcardDomainValidator wildcardDomainValidator)
        : base(new OpenIddictServerHandlers.Authentication.ValidateClientRedirectUri(applicationManager), wildcardDomainValidator)
    {
    }

    public async override ValueTask HandleAsync(OpenIddictServerEvents.ValidateAuthorizationRequestContext context)
    {
        Check.NotNull(context, nameof(context));

        if (!string.IsNullOrEmpty(context.RedirectUri) && await WildcardDomainValidator.IsValidAsync(context.RedirectUri, context.ClientId))
        {
            return;
        }

        await OriginalHandler.HandleAsync(context);
    }
}
