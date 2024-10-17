using System.Threading.Tasks;
using Microsoft.Extensions.Options;
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
        IOptions<AbpOpenIddictWildcardDomainOptions> wildcardDomainsOptions,
        IOpenIddictApplicationManager applicationManager)
        : base(wildcardDomainsOptions, new OpenIddictServerHandlers.Session.ValidateAuthorizedParty(applicationManager))
    {
        OriginalHandler = new OpenIddictServerHandlers.Session.ValidateAuthorizedParty(applicationManager);
    }

    public async override ValueTask HandleAsync(OpenIddictServerEvents.ValidateEndSessionRequestContext context)
    {
        Check.NotNull(context, nameof(context));

        if (await CheckWildcardDomainAsync(context.PostLogoutRedirectUri))
        {
            return;
        }

        await OriginalHandler.HandleAsync(context);
    }
}
