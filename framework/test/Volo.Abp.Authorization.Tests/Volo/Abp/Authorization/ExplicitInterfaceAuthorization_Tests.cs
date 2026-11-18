using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.Authorization;

public class ExplicitInterfaceAuthorization_Tests : AuthorizationTestBase
{
    [Fact]
    public async Task Should_Require_Authentication_For_An_Explicit_Interface_Method()
    {
        var service = GetRequiredService<IExplicitlyAuthorizedService>();
        using (GetRequiredService<ICurrentPrincipalAccessor>().Change(new ClaimsPrincipal(new ClaimsIdentity())))
        {
            await Should.ThrowAsync<AbpAuthorizationException>(() => service.ReadAsync());
        }
    }

    [Fact]
    public async Task Should_Allow_An_Authenticated_Interface_Caller()
    {
        var service = GetRequiredService<IExplicitlyAuthorizedService>();
        using (GetRequiredService<ICurrentPrincipalAccessor>().Change(new ClaimsPrincipal(new ClaimsIdentity(
                   new[] { new Claim(AbpClaimTypes.UserId, "1fcf46b2-28c3-48d0-8bac-fa53268a2775") }, "Test"))))
        {
            (await service.ReadAsync()).ShouldBe(42);
        }
    }

    [Fact]
    public async Task Should_Keep_Protected_Helpers_Available_From_Anonymous_Methods()
    {
        var service = GetRequiredService<ExplicitlyAuthorizedService>();
        using (GetRequiredService<ICurrentPrincipalAccessor>().Change(new ClaimsPrincipal(new ClaimsIdentity())))
        {
            (await service.AnonymousAsync()).ShouldBe(42);
        }
    }
}

public interface IExplicitlyAuthorizedService
{
    Task<int> ReadAsync();
}

[Authorize]
public class ExplicitlyAuthorizedService : IExplicitlyAuthorizedService, ITransientDependency
{
    Task<int> IExplicitlyAuthorizedService.ReadAsync() => Task.FromResult(42);

    [AllowAnonymous]
    public virtual Task<int> AnonymousAsync() => ReadInternalAsync();

    protected virtual Task<int> ReadInternalAsync() => Task.FromResult(42);
}
