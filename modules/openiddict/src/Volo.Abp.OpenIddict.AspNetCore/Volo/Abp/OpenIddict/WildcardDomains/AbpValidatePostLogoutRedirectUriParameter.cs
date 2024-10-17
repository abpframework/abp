using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public class AbpValidatePostLogoutRedirectUriParameter : AbpOpenIddictWildcardDomainBase<AbpValidatePostLogoutRedirectUriParameter, OpenIddictServerHandlers.Session.ValidatePostLogoutRedirectUriParameter, OpenIddictServerEvents.ValidateEndSessionRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateEndSessionRequestContext>()
            .UseSingletonHandler<AbpValidatePostLogoutRedirectUriParameter>()
            .SetOrder(int.MinValue + 100_000)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    public AbpValidatePostLogoutRedirectUriParameter(IOptions<AbpOpenIddictWildcardDomainOptions> wildcardDomainsOptions)
        : base(wildcardDomainsOptions, new OpenIddictServerHandlers.Session.ValidatePostLogoutRedirectUriParameter())
    {
    }

    public async override ValueTask HandleAsync(OpenIddictServerEvents.ValidateEndSessionRequestContext context)
    {
        Check.NotNull(context, nameof(context));

        if (string.IsNullOrEmpty(context.PostLogoutRedirectUri) || await CheckWildcardDomainAsync(context.PostLogoutRedirectUri))
        {
            return;
        }

        await OriginalHandler.HandleAsync(context);
    }
}
