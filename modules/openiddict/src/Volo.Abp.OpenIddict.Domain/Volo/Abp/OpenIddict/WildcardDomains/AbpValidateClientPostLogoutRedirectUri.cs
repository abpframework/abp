using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public class AbpValidateClientPostLogoutRedirectUri : AbpOpenIddictWildcardDomainBase<OpenIddictServerHandlers.Session.ValidateClientPostLogoutRedirectUri, OpenIddictServerEvents.ValidateLogoutRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateLogoutRequestContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireDegradedModeDisabled>()
            .AddFilter<OpenIddictServerHandlerFilters.RequirePostLogoutRedirectUriParameter>()
            .UseScopedHandler<AbpValidateClientPostLogoutRedirectUri>()
            .SetOrder(OpenIddictServerHandlers.Session.ValidatePostLogoutRedirectUriParameter.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    public AbpValidateClientPostLogoutRedirectUri(
        IOptions<AbpOpenIddictWildcardDomainOptions> wildcardDomainsOptions,
        IOpenIddictApplicationManager applicationManager)
        : base(wildcardDomainsOptions, false)
    {
        Handler = new OpenIddictServerHandlers.Session.ValidateClientPostLogoutRedirectUri(applicationManager);
    }

    public async override ValueTask HandleAsync(OpenIddictServerEvents.ValidateLogoutRequestContext context)
    {
        Check.NotNull(context, nameof(context));
        Check.NotNullOrEmpty(context.PostLogoutRedirectUri, nameof(context.PostLogoutRedirectUri));

        if (await CheckWildcardDomainAsync(context.PostLogoutRedirectUri))
        {
            return;
        }

        await Handler.HandleAsync(context);
    }
}
