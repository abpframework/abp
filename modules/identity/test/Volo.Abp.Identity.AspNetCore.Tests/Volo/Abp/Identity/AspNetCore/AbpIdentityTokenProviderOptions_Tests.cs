using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpIdentityTokenProviderOptions_Tests
{
    [Fact]
    public async Task Should_Register_Abp_Token_Providers_Without_The_AspNetCore_Module()
    {
        var tokens = await GetTokenOptionsAsync<DomainOnlyModule>();

        tokens.PasswordResetTokenProvider.ShouldBe(AbpPasswordResetTokenProvider.ProviderName);
        tokens.EmailConfirmationTokenProvider.ShouldBe(AbpEmailConfirmationTokenProvider.ProviderName);
        tokens.ChangeEmailTokenProvider.ShouldBe(AbpChangeEmailTokenProvider.ProviderName);

        tokens.ProviderMap[TokenOptions.DefaultProvider].ProviderType.ShouldBe(typeof(AbpDefaultTokenProvider));
        tokens.ProviderMap[TokenOptions.DefaultEmailProvider].ProviderType.ShouldBe(typeof(AbpEmailTwoFactorTokenProvider));
        tokens.ProviderMap[TokenOptions.DefaultPhoneProvider].ProviderType.ShouldBe(typeof(AbpPhoneNumberTwoFactorTokenProvider));
        tokens.ProviderMap[LinkUserTokenProviderConsts.LinkUserTokenProviderName].ProviderType.ShouldBe(typeof(LinkUserTokenProvider));

        // Not replaced by ABP, but still has to be registered: the providers are listed one by one
        // instead of calling AddDefaultTokenProviders(), so a missing key would go unnoticed.
        tokens.ProviderMap[TokenOptions.DefaultAuthenticatorProvider].ProviderType
            .ShouldBe(typeof(AuthenticatorTokenProvider<IdentityUser>));
    }

    [Fact]
    public async Task Should_Resolve_The_Same_Providers_With_And_Without_The_AspNetCore_Module()
    {
        var domainOnly = await GetTokenOptionsAsync<DomainOnlyModule>();
        var withAspNetCore = await GetTokenOptionsAsync<AspNetCoreModule>();

        withAspNetCore.PasswordResetTokenProvider.ShouldBe(domainOnly.PasswordResetTokenProvider);
        withAspNetCore.EmailConfirmationTokenProvider.ShouldBe(domainOnly.EmailConfirmationTokenProvider);
        withAspNetCore.ChangeEmailTokenProvider.ShouldBe(domainOnly.ChangeEmailTokenProvider);

        // Both directions: iterating one map alone would pass while the other side has extra keys.
        withAspNetCore.ProviderMap.Keys.OrderBy(x => x).ShouldBe(domainOnly.ProviderMap.Keys.OrderBy(x => x));

        foreach (var (name, descriptor) in domainOnly.ProviderMap)
        {
            withAspNetCore.ProviderMap[name].ProviderType.ShouldBe(descriptor.ProviderType);
        }
    }

    [Theory]
    [InlineData(typeof(NoAbpTokenProvidersModule))]
    [InlineData(typeof(DomainOnlyNoAbpTokenProvidersModule))]
    public async Task Should_Register_No_Token_Provider_When_Opted_Out(Type moduleType)
    {
        // Registering something here for one host shape and nothing for the other would put the two
        // ends of a link back on different providers, which is the failure the flag has to avoid.
        var tokens = await GetTokenOptionsAsync(moduleType);

        tokens.ProviderMap.ShouldBeEmpty();

        tokens.PasswordResetTokenProvider.ShouldBe(TokenOptions.DefaultProvider);
        tokens.EmailConfirmationTokenProvider.ShouldBe(TokenOptions.DefaultProvider);
        tokens.ChangeEmailTokenProvider.ShouldBe(TokenOptions.DefaultProvider);
    }

    [Theory]
    [InlineData(typeof(AspNetCoreStockTokenProvidersModule))]
    [InlineData(typeof(DomainOnlyStockTokenProvidersModule))]
    public async Task Should_Let_The_Application_Register_The_AspNetCore_Providers_When_Opted_Out(Type moduleType)
    {
        var tokens = await GetTokenOptionsAsync(moduleType);

        tokens.ProviderMap[TokenOptions.DefaultProvider].ProviderType.ShouldBe(typeof(DataProtectorTokenProvider<IdentityUser>));
        tokens.ProviderMap[TokenOptions.DefaultEmailProvider].ProviderType.ShouldBe(typeof(EmailTokenProvider<IdentityUser>));
        tokens.ProviderMap[TokenOptions.DefaultPhoneProvider].ProviderType.ShouldBe(typeof(PhoneNumberTokenProvider<IdentityUser>));
        tokens.ProviderMap[TokenOptions.DefaultAuthenticatorProvider].ProviderType.ShouldBe(typeof(AuthenticatorTokenProvider<IdentityUser>));
    }

    [Fact]
    public async Task Should_Let_The_Application_Override_A_Provider()
    {
        var tokens = await GetTokenOptionsAsync<CustomPasswordResetProviderModule>();

        tokens.ProviderMap[AbpPasswordResetTokenProvider.ProviderName].ProviderType
            .ShouldBe(typeof(DataProtectorTokenProvider<IdentityUser>));
    }

    [Fact]
    public async Task Should_Register_DataProtection_For_The_Token_Providers()
    {
        // UserManager instantiates every provider in Tokens.ProviderMap when it is resolved, and the
        // DataProtection based ones need IDataProtectionProvider. Hosts that never issue a token, such
        // as a DbMigrator console application, do not register it themselves.
        using var application = await AbpApplicationFactory.CreateAsync<DomainOnlyModule>();
        await application.InitializeAsync();

        application.ServiceProvider.GetService<IDataProtectionProvider>().ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Register_DataProtection_When_Opted_Out()
    {
        // Opting out is not a reason to take it away: the providers the application registers instead
        // are typically the ASP.NET Core ones, and DataProtectorTokenProvider needs it just as much.
        using var application = await AbpApplicationFactory.CreateAsync<NoAbpTokenProvidersModule>();
        await application.InitializeAsync();

        application.ServiceProvider.GetService<IDataProtectionProvider>().ShouldNotBeNull();
    }

    [Fact]
    public async Task A_Leftover_AddDefaultTokenProviders_Should_Override_Only_The_Keys_It_Registers()
    {
        // Application actions run after the framework registration, so a host that also calls
        // AddDefaultTokenProviders() ends up with the stock providers on the keys that call covers
        // and the ABP ones everywhere else.
        var tokens = await GetTokenOptionsAsync<LeftoverDefaultTokenProvidersModule>();

        tokens.ProviderMap[TokenOptions.DefaultProvider].ProviderType.ShouldBe(typeof(DataProtectorTokenProvider<IdentityUser>));
        tokens.ProviderMap[TokenOptions.DefaultEmailProvider].ProviderType.ShouldBe(typeof(EmailTokenProvider<IdentityUser>));
        tokens.ProviderMap[TokenOptions.DefaultPhoneProvider].ProviderType.ShouldBe(typeof(PhoneNumberTokenProvider<IdentityUser>));

        tokens.PasswordResetTokenProvider.ShouldBe(AbpPasswordResetTokenProvider.ProviderName);
        tokens.ProviderMap[AbpPasswordResetTokenProvider.ProviderName].ProviderType.ShouldBe(typeof(AbpPasswordResetTokenProvider));
    }

    private static Task<TokenOptions> GetTokenOptionsAsync<TModule>()
        where TModule : IAbpModule
    {
        return GetTokenOptionsAsync(typeof(TModule));
    }

    private static async Task<TokenOptions> GetTokenOptionsAsync(Type moduleType)
    {
        using var application = await AbpApplicationFactory.CreateAsync(moduleType);
        await application.InitializeAsync();
        return application.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value.Tokens;
    }
}

[DependsOn(typeof(AbpIdentityDomainModule))]
public class DomainOnlyModule : AbpModule
{
}

[DependsOn(typeof(AbpIdentityAspNetCoreModule))]
public class AspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityAspNetCoreOptions>(options => options.ConfigureAuthentication = false);
    }
}

[DependsOn(typeof(AbpIdentityDomainModule))]
public class LeftoverDefaultTokenProvidersModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder => builder.AddDefaultTokenProviders());
    }
}

[DependsOn(typeof(AbpIdentityDomainModule))]
public class DomainOnlyNoAbpTokenProvidersModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityTokenProviderOptions>(options => options.UseAbpTokenProviders = false);
    }
}

[DependsOn(typeof(AbpIdentityAspNetCoreModule))]
public class NoAbpTokenProvidersModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityAspNetCoreOptions>(options => options.ConfigureAuthentication = false);
        PreConfigure<AbpIdentityTokenProviderOptions>(options => options.UseAbpTokenProviders = false);
    }
}

[DependsOn(typeof(AbpIdentityAspNetCoreModule))]
public class AspNetCoreStockTokenProvidersModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityAspNetCoreOptions>(options => options.ConfigureAuthentication = false);
        PreConfigure<AbpIdentityTokenProviderOptions>(options => options.UseAbpTokenProviders = false);
        PreConfigure<IdentityBuilder>(builder => builder.AddDefaultTokenProviders());
    }
}

[DependsOn(typeof(AbpIdentityDomainModule))]
public class DomainOnlyStockTokenProvidersModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityTokenProviderOptions>(options => options.UseAbpTokenProviders = false);
        PreConfigure<IdentityBuilder>(builder => builder.AddDefaultTokenProviders());
    }
}

[DependsOn(typeof(AbpIdentityDomainModule))]
public class CustomPasswordResetProviderModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder =>
            builder.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(AbpPasswordResetTokenProvider.ProviderName));
    }
}
