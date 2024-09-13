using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict.Globalization;

public class AttachCultureInfo : IOpenIddictServerHandler<OpenIddictServerEvents.ApplyAuthorizationResponseContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ApplyAuthorizationResponseContext>()
            .UseSingletonHandler<AttachCultureInfo>()
            .SetOrder(OpenIddictServerHandlers.Authentication.AttachIssuer.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    protected IOptions<AbpOpenIddictAspNetCoreOptions> Options { get; }

    public AttachCultureInfo(IOptions<AbpOpenIddictAspNetCoreOptions> options)
    {
        Options = options;
    }

    public ValueTask HandleAsync(OpenIddictServerEvents.ApplyAuthorizationResponseContext context)
    {
        Check.NotNull(context, nameof(context));

        if (Options.Value.AttachCultureInfo)
        {
            if (!context.Response.HasParameter("culture"))
            {
                context.Response.SetParameter("culture", CultureInfo.CurrentCulture.Name);
            }

            if (!context.Response.HasParameter("ui-culture"))
            {
                context.Response.SetParameter("ui-culture", CultureInfo.CurrentUICulture.Name);
            }
        }

        return default;
    }
}
