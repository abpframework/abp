using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Mvc.Authorization
{
    public class FakeAuthenticationMiddleware : IMiddleware, ITransientDependency
    {
        private readonly FakeUserClaims _fakeUserClaims;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public FakeAuthenticationMiddleware(FakeUserClaims fakeUserClaims, ICurrentPrincipalAccessor currentPrincipalAccessor)
        {
            _fakeUserClaims = fakeUserClaims;
            _currentPrincipalAccessor = currentPrincipalAccessor;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_fakeUserClaims.Claims.Any())
            {
                context.User = new ClaimsPrincipal(new List<ClaimsIdentity>
                {
                    new ClaimsIdentity(_fakeUserClaims.Claims, "FakeSchema")
                });

                //_currentPrincipalAccessor.Change(context.User);
            }

            await next(context);
        }
    }
}
