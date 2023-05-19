using Microsoft.AspNetCore.Http;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;

namespace Volo.Abp.OpenIddict;

public static class AbpOpenIddictHttpContextExtensions
{
    public static OpenIddictServerTransaction GetOpenIddictServerTransaction(this HttpContext context)
    {
        Check.NotNull(context, nameof(context));
        return context.Features.Get<OpenIddictServerAspNetCoreFeature>()?.Transaction;
    }
}
