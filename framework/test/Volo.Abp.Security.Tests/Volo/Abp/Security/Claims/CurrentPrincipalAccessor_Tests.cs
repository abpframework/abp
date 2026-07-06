using System.Collections.Generic;
using System.Security.Claims;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Security.Claims;

public class CurrentPrincipalAccessor_Tests : AbpIntegratedTest<AbpSecurityTestModule>
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public CurrentPrincipalAccessor_Tests()
    {
        _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
    }

    [Fact]
    public void Should_Get_Changed_Principal_If()
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name,"bob"),
                new Claim(ClaimTypes.NameIdentifier,"123456")
            }));

        var claimsPrincipal2 = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name,"lee"),
                new Claim(ClaimTypes.NameIdentifier,"654321")
            }));


        var anonymousPrincipal = _currentPrincipalAccessor.Principal;
        anonymousPrincipal.ShouldNotBeNull();
        anonymousPrincipal.Identity.ShouldNotBeNull();
        anonymousPrincipal.Identity.IsAuthenticated.ShouldBeFalse();

        using (_currentPrincipalAccessor.Change(claimsPrincipal))
        {
            _currentPrincipalAccessor.Principal.ShouldBe(claimsPrincipal);

            using (_currentPrincipalAccessor.Change(claimsPrincipal2))
            {
                _currentPrincipalAccessor.Principal.ShouldBe(claimsPrincipal2);
            }

            _currentPrincipalAccessor.Principal.ShouldBe(claimsPrincipal);
        }
        var currentPrincipal = _currentPrincipalAccessor.Principal;
        currentPrincipal.ShouldNotBeNull();
        currentPrincipal.Identity.ShouldNotBeNull();
        currentPrincipal.Identity.IsAuthenticated.ShouldBeFalse();
    }

    [Fact]
    public void Should_Reflect_Underlying_Source_After_Change_Scope_Disposed()
    {
        var accessor = new TestCurrentPrincipalAccessor();

        var changedPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "123456")
        }));

        using (accessor.Change(changedPrincipal))
        {
            accessor.Principal.ShouldBe(changedPrincipal);
        }

        // Disposing the Change scope must not pin the accessor to the fallback principal;
        // a principal that becomes available afterwards has to be reflected.
        var sourcePrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "654321")
        }));
        accessor.SourcePrincipal = sourcePrincipal;

        accessor.Principal.ShouldBe(sourcePrincipal);
    }

    private class TestCurrentPrincipalAccessor : CurrentPrincipalAccessorBase
    {
        public ClaimsPrincipal? SourcePrincipal { get; set; }

        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            return SourcePrincipal ?? new ClaimsPrincipal(new ClaimsIdentity());
        }
    }
}
