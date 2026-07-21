using System;
using System.Collections.Generic;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity;

[Dependency(ReplaceServices = true)]
public class FakeCurrentPrincipalAccessor : ThreadCurrentPrincipalAccessor
{
    private readonly IdentityTestData _testData;
    private readonly Lazy<ClaimsPrincipal> _principal;

    public FakeCurrentPrincipalAccessor(IdentityTestData testData)
    {
        _testData = testData;
        _principal = new Lazy<ClaimsPrincipal>(() => new ClaimsPrincipal(
            new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(AbpClaimTypes.UserId, _testData.UserAdminId.ToString()),
                    new Claim(AbpClaimTypes.UserName, "administrator"),
                    new Claim(AbpClaimTypes.Email, "administrator@abp.io")
                }
            )
        ));
    }

    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        return GetPrincipal();
    }

    private ClaimsPrincipal GetPrincipal()
    {
        return _principal.Value;
    }
}
