using System.Threading.Tasks;
using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict;

public class RemoveClaimsFromClientCredentialsGrantType : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessSignInContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ProcessSignInContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireAccessTokenGenerated>()
            .UseSingletonHandler<RemoveClaimsFromClientCredentialsGrantType>()
            .SetOrder(OpenIddictServerHandlers.PrepareAccessTokenPrincipal.Descriptor.Order - 1)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public virtual ValueTask HandleAsync(OpenIddictServerEvents.ProcessSignInContext context)
    {
        if (context.Request.IsClientCredentialsGrantType())
        {
            if (context.Principal != null)
            {
                context.Principal.RemoveClaims(OpenIddictConstants.Claims.Subject);
                context.Principal.RemoveClaims(OpenIddictConstants.Claims.PreferredUsername);
            }
        }

        return default;
    }
}
