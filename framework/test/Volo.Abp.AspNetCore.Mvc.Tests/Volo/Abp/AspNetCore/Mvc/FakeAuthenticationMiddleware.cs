using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.AspNetCore.Middleware;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc;

[Obsolete("Use FakeAuthenticationScheme instead.")]
public class FakeAuthenticationMiddleware : AbpMiddlewareBase, ITransientDependency
{
    private readonly FakeUserClaims _fakeUserClaims;

    public FakeAuthenticationMiddleware(FakeUserClaims fakeUserClaims)
    {
        _fakeUserClaims = fakeUserClaims;
    }

    public async override Task InvokeAsync(HttpContext context, RequestDelegate next)
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
