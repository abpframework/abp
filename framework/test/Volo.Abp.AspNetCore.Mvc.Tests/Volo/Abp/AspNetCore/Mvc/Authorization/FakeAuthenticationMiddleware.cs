using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Authorization
{
    public class FakeAuthenticationMiddleware : IMiddleware, ITransientDependency
    {
        private readonly FakeUserClaims _fakeUserClaims;

        public FakeAuthenticationMiddleware(FakeUserClaims fakeUserClaims)
        {
            _fakeUserClaims = fakeUserClaims;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_fakeUserClaims.Claims.Any())
            {
                context.User = new ClaimsPrincipal(new List<ClaimsIdentity>
                {
                    new ClaimsIdentity(_fakeUserClaims.Claims, "FakeSchema")
                });
            }

            await next(context);
        }
    }
}
