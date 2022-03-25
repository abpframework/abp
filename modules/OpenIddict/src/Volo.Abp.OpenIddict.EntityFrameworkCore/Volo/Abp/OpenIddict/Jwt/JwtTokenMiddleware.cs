using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;

namespace Volo.Abp.OpenIddict.Jwt;

//TODO: Should we move this to another package..?

public static class JwtTokenMiddleware
{
    public static IApplicationBuilder UseJwtTokenMiddleware(this IApplicationBuilder app, string schema)
    {
        return app.Use(async (ctx, next) =>
        {
            if (ctx.User.Identity?.IsAuthenticated != true)
            {
                var result = await ctx.AuthenticateAsync(schema);
                if (result.Succeeded && result.Principal != null)
                {
                    ctx.User = result.Principal;
                }
            }

            await next();
        });
    }
}
