using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// The validating host decides whether a token is expired, so a changed default silently shortens or
/// extends every outstanding link across the solution. Pinned here, resolved through the options
/// pipeline so that a stray <c>Configure</c> in the module is caught as well.
/// </summary>
public class TokenProviderLifespan_Tests
{
    [Fact]
    public async Task Default_Lifespans_Should_Not_Change()
    {
        using var application = await AbpApplicationFactory.CreateAsync<DomainOnlyModule>();
        await application.InitializeAsync();

        LifespanOf<AbpPasswordResetTokenProviderOptions>(application).ShouldBe(TimeSpan.FromHours(2));
        LifespanOf<AbpEmailConfirmationTokenProviderOptions>(application).ShouldBe(TimeSpan.FromHours(2));
        LifespanOf<AbpChangeEmailTokenProviderOptions>(application).ShouldBe(TimeSpan.FromHours(2));
        LifespanOf<AbpDefaultTokenProviderOptions>(application).ShouldBe(TimeSpan.FromMinutes(10));
        LifespanOf<AbpLinkUserTokenProviderOptions>(application).ShouldBe(TimeSpan.FromMinutes(10));

        application.ServiceProvider.GetRequiredService<IOptions<AbpEmailTwoFactorTokenProviderOptions>>()
            .Value.TokenLifespan.ShouldBe(TimeSpan.FromMinutes(3));
        application.ServiceProvider.GetRequiredService<IOptions<AbpPhoneNumberTwoFactorTokenProviderOptions>>()
            .Value.TokenLifespan.ShouldBe(TimeSpan.FromMinutes(3));
    }

    [Fact]
    public void The_Shared_Default_Should_Match_The_AspNetCore_One()
    {
        // Compared against the live ASP.NET Core defaults, so that a change on their side shows up here
        // instead of silently moving every provider that does not set its own lifespan.
        var aspNetCore = new DataProtectionTokenProviderOptions();
        var abp = new UnconfiguredTokenProviderOptions();

        abp.Name.ShouldBe(aspNetCore.Name);
        abp.TokenLifespan.ShouldBe(aspNetCore.TokenLifespan);
    }

    private static TimeSpan LifespanOf<TOptions>(IAbpApplication application)
        where TOptions : AbpDataProtectionTokenProviderOptions, new()
    {
        return application.ServiceProvider.GetRequiredService<IOptions<TOptions>>().Value.TokenLifespan;
    }

    private class UnconfiguredTokenProviderOptions : AbpDataProtectionTokenProviderOptions
    {
    }
}
