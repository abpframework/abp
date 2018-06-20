using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Mvc.Authorization
{
    public class FakeAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FakeUserClaims _fakeUserClaims;

        public FakeAuthenticationMiddleware(RequestDelegate next, FakeUserClaims fakeUserClaims)
        {
            _next = next;
            _fakeUserClaims = fakeUserClaims;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (_fakeUserClaims.Claims.Any())
            {
                httpContext.User = new ClaimsPrincipal(new List<ClaimsIdentity>
                {
                    new ClaimsIdentity(_fakeUserClaims.Claims, "FakeSchema")
                });
            }

            await _next(httpContext);
        }
    }
}