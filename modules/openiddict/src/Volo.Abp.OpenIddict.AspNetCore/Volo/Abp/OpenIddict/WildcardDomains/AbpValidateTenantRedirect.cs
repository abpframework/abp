using System.Threading.Tasks;
using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public class AbpValidateTenantRedirect : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessSignInContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; } =
        OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ProcessSignInContext>()
            .UseScopedHandler<AbpValidateTenantRedirect>()
            .SetOrder(int.MinValue + 100_000)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    protected AbpOpenIddictWildcardDomainValidator Validator { get; }

    public AbpValidateTenantRedirect(AbpOpenIddictWildcardDomainValidator validator)
    {
        Validator = validator;
    }

    public virtual async ValueTask HandleAsync(OpenIddictServerEvents.ProcessSignInContext context)
    {
        // Request caching signs in an empty principal to create the request_uri; codes are only issued to authenticated users.
        if (context.EndpointType != OpenIddictServerEndpointType.Authorization ||
            context.Principal?.Identity?.IsAuthenticated != true)
        {
            return;
        }

        // OpenIddict redirects to the URI resolved during request validation, which may differ from the request parameter.
        var redirectUri = context.Transaction.GetProperty<OpenIddictServerEvents.ValidateAuthorizationRequestContext>(
            typeof(OpenIddictServerEvents.ValidateAuthorizationRequestContext).FullName!)?.RedirectUri ?? context.Request.RedirectUri;
        if (string.IsNullOrEmpty(redirectUri))
        {
            return;
        }

        if (!await Validator.IsValidForPrincipalAsync(redirectUri, context.Request.ClientId, context.Principal))
        {
            context.Reject(OpenIddictConstants.Errors.InvalidRequest, "The redirect URI is not valid for this tenant.");
        }
    }
}
