using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Security.Claims
{
    public class AbpClaimsMapMiddleware : IMiddleware, ITransientDependency
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var currentPrincipalAccessor = context.RequestServices.GetRequiredService<ICurrentPrincipalAccessor>();
            var mapOptions = context.RequestServices.GetRequiredService<IOptions<AbpClaimsMapOptions>>().Value;

            var mapClaims = currentPrincipalAccessor.Principal.Claims.Where(p => mapOptions.Maps.Keys.Contains(p.Type));
            currentPrincipalAccessor.Principal.AddIdentity(new ClaimsIdentity(mapClaims.Select(p => new Claim(
                mapOptions.Maps[p.Type],
                p.Value,
                p.ValueType,
                p.Issuer))));

            await next(context);
        }
    }
}
