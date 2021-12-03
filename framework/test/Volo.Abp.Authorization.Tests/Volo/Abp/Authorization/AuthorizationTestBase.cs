using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;

namespace Volo.Abp.Authorization;

public class AuthorizationTestBase : AbpIntegratedTest<AbpAuthorizationTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        var claims = new List<Claim>() {
                                new Claim(AbpClaimTypes.UserName, "Douglas"),
                                new Claim(AbpClaimTypes.UserId, "1fcf46b2-28c3-48d0-8bac-fa53268a2775"),
                                new Claim(AbpClaimTypes.Role, "MyRole")
                            };

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var principalAccessor = Substitute.For<ICurrentPrincipalAccessor>();
        principalAccessor.Principal.Returns(ci => claimsPrincipal);
        Thread.CurrentPrincipal = claimsPrincipal;
    }
}
