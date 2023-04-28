using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shouldly;
using Volo.Abp.Authorization.Fluent;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
using Xunit;

namespace Volo.Abp.Authorization;

public class FluentAuthorization_Tests : AuthorizationTestBase
{
    private readonly IAuthorizationService _authorizationService;

    public FluentAuthorization_Tests()
    {
        _authorizationService = GetRequiredService<IAuthorizationService>();
    }

    [Fact]
    public async Task IsGrantedAsync_Should_Work_With_Fluent_Authorization()
    {
        (await _authorizationService.IsGrantedAsync(x => x // false
            .Meet(y =>
            {
                y.IsGrantedAny(new[] { "MyPermission1", "MyPermission3" }); // true
                y.Condition(() => false); // false
            })
            .OrMeet(y => // true
            {
                y.IsGranted(new[] { "MyPermission3", "MyPermission5" }); // true
                y.HasClaim(AbpClaimTypes.UserName, "Douglas"); // true
                y.IsUser(Guid.Parse("1fcf46b2-28c3-48d0-8bac-fa53268a2775")); // true
                y.IsUser("Douglas"); // true
                y.IsInUsers(new[] { Guid.Parse("1fcf46b2-28c3-48d0-8bac-fa53268a2775"), Guid.NewGuid() }); // true
                y.IsInUsers(new[] { "Douglas", "Amy" }); // true
                y.IsInRole("MyRole"); // true
                y.IsInAnyRole(new[] { "guest", "MyRole" }); // true
                y.NotMeet(z => z.IsInAllRoles(new[] { "guest", "MyRole" })); // true
                y.NotMeet(z => z.HasClaim("SomeInvalidClaim")); // true
            })
        )).ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsInRole("MyRole")))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsInRole("admin")))
            .ShouldBe(false);

        (await _authorizationService.IsGrantedAsync(x => x.IsInAllRoles(new[] { "admin", "MyRole" })))
            .ShouldBe(false);

        (await _authorizationService.IsGrantedAsync(x => x.NotMeet(y => y.IsInRole("guest"))))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsInAnyRole(new[] { "guest", "MyRole" })))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.NotMeet(y => y.IsInAllRoles(new[] { "guest", "MyRole" }))))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsGrantedAny(new[] { "MyPermission1", "MyPermission3" })))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsGranted(new[] { "MyPermission3", "MyPermission5" })))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsGranted("MyPermission1")))
            .ShouldBe(false);

        (await _authorizationService.IsGrantedAsync(x => x.Condition(() => true)))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.Condition(() => false)))
            .ShouldBe(false);

        (await _authorizationService.IsGrantedAsync(x => x.Condition(async () => await Task.FromResult(true))))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.HasClaim(AbpClaimTypes.UserName)))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.HasClaim(AbpClaimTypes.UserName, "Douglas")))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.HasClaim(AbpClaimTypes.UserName, new[] { "Douglas" })))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsUser(Guid.Parse("1fcf46b2-28c3-48d0-8bac-fa53268a2775"))))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsUser("Douglas")))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsUser(Guid.NewGuid())))
            .ShouldBe(false);

        (await _authorizationService.IsGrantedAsync(x => x.IsUser("Amy")))
            .ShouldBe(false);

        (await _authorizationService.IsGrantedAsync(x =>
                x.IsInUsers(new[] { Guid.Parse("1fcf46b2-28c3-48d0-8bac-fa53268a2775"), Guid.NewGuid() })))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsInUsers(new[] { Guid.NewGuid(), Guid.NewGuid() })))
            .ShouldBe(false);

        (await _authorizationService.IsGrantedAsync(x => x.IsInUsers(new[] { "Douglas", "Amy" })))
            .ShouldBe(true);

        (await _authorizationService.IsGrantedAsync(x => x.IsInUsers(new[] { "John", "Amy" })))
            .ShouldBe(false);
    }

    [Fact]
    public async Task CheckAsync_Should_Work_With_Fluent_Authorization()
    {
        await Should.NotThrowAsync(_authorizationService.CheckAsync(x => x
            .Meet(y =>
            {
                y.IsGrantedAny(new[] { "MyPermission1", "MyPermission3" }); // true
                y.Condition(() => false); // false
            })
            .OrMeet(y => // true
            {
                y.IsGranted(new[] { "MyPermission3", "MyPermission5" }); // true
                y.HasClaim(AbpClaimTypes.UserName, "Douglas"); // true
                y.IsUser(Guid.Parse("1fcf46b2-28c3-48d0-8bac-fa53268a2775")); // true
                y.IsUser("Douglas"); // true
                y.IsInUsers(new[] { Guid.Parse("1fcf46b2-28c3-48d0-8bac-fa53268a2775"), Guid.NewGuid() }); // true
                y.IsInUsers(new[] { "Douglas", "Amy" }); // true
                y.IsInRole("MyRole"); // true
                y.IsInAnyRole(new[] { "guest", "MyRole" }); // true
                y.NotMeet(z => z.IsInAllRoles(new[] { "guest", "MyRole" })); // true
                y.NotMeet(z => z.HasClaim("SomeInvalidClaim")); // true
            })
        ));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.IsInRole("MyRole")));

        await Should.ThrowAsync<AbpAuthorizationException>(
            _authorizationService.CheckAsync(x => x.IsInRole("admin")));

        await Should.ThrowAsync<AbpAuthorizationException>(
            _authorizationService.CheckAsync(x => x.IsInAllRoles(new[] { "admin", "MyRole" })));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.NotMeet(y => y.IsInRole("guest"))));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.IsInAnyRole(new[] { "guest", "MyRole" })));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.NotMeet(y => y.IsInAllRoles(new[] { "guest", "MyRole" }))));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.IsGrantedAny(new[] { "MyPermission1", "MyPermission3" })));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.IsGranted(new[] { "MyPermission3", "MyPermission5" })));

        await Should.ThrowAsync<AbpAuthorizationException>(
            _authorizationService.CheckAsync(x => x.IsGranted("MyPermission1")));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.Condition(() => true)));

        await Should.ThrowAsync<AbpAuthorizationException>(
            _authorizationService.CheckAsync(x => x.Condition(() => false)));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.Condition(async () => await Task.FromResult(true))));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.HasClaim(AbpClaimTypes.UserName)));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.HasClaim(AbpClaimTypes.UserName, "Douglas")));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.HasClaim(AbpClaimTypes.UserName, new[] { "Douglas" })));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.IsUser(Guid.Parse("1fcf46b2-28c3-48d0-8bac-fa53268a2775"))));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.IsUser("Douglas")));

        await Should.ThrowAsync<AbpAuthorizationException>(
            _authorizationService.CheckAsync(x => x.IsUser(Guid.NewGuid())));

        await Should.ThrowAsync<AbpAuthorizationException>(
            _authorizationService.CheckAsync(x => x.IsUser("Amy")));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x =>
                x.IsInUsers(new[] { Guid.Parse("1fcf46b2-28c3-48d0-8bac-fa53268a2775"), Guid.NewGuid(), })));

        await Should.ThrowAsync<AbpAuthorizationException>(
            _authorizationService.CheckAsync(x => x.IsInUsers(new[] { Guid.NewGuid(), Guid.NewGuid(), })));

        await Should.NotThrowAsync(
            _authorizationService.CheckAsync(x => x.IsInUsers(new[] { "Douglas", "Amy" })));

        await Should.ThrowAsync<AbpAuthorizationException>(
            _authorizationService.CheckAsync(x => x.IsInUsers(new[] { "John", "Amy" })));
    }

    [Fact]
    public async Task Should_Work_Without_Fluent_Authorization()
    {
        var currentUser = GetRequiredService<ICurrentUser>();
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        var result = ((await _authorizationService.IsGrantedAsync("MyPermission3") ||
                       await _authorizationService.IsGrantedAsync("MyPermission5")) && false) ||
                     (await _authorizationService.IsGrantedAsync("MyPermission3") &&
                      await _authorizationService.IsGrantedAsync("MyPermission5") &&
                      currentUser.Id == Guid.Parse("1fcf46b2-28c3-48d0-8bac-fa53268a2775") &&
                      currentUser.UserName == "Douglas" &&
                      new[] { Guid.Parse("1fcf46b2-28c3-48d0-8bac-fa53268a2775"), Guid.NewGuid() }
                          .Contains(currentUser.Id.Value) &&
                      new[] { "Douglas", "Amy" }.Contains(currentUser.UserName) &&
                      currentUser.IsInRole("MyRole") &&
                      (currentUser.IsInRole("guest") || currentUser.IsInRole("MyRole")) &&
                      !(currentUser.IsInRole("guest") && currentUser.IsInRole("MyRole")) &&
                      currentUser.FindClaim("SomeInvalidClaim") == null);
        result.ShouldBe(true);
    }
}