using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.EntityFrameworkCore;

/// <summary>
/// The Entity Framework Core run of the shared cross-host suite, plus the cases that do not depend on
/// the persistence provider and therefore only need to run once.
/// </summary>
public class CrossHostTokenProvider_Tests
    : CrossHostTokenProvider_Tests<CrossHostGeneratorHostModule, CrossHostValidatorHostModule>
{
    protected override Type ValidatorOnlyModuleType => typeof(AbpIdentityAspNetCoreModule);

    protected override IDisposable CreateSharedDatabase()
    {
        var database = new AbpUnitTestSqliteDatabase();
        database.CreateTables(
            new IdentityDbContext(new DbContextOptionsBuilder<IdentityDbContext>().UseSqlite(database.ConnectionString).Options));

        CrossHostTokenProviderTestModuleBase.ConnectionString = database.ConnectionString;
        return database;
    }

    [Fact]
    public async Task A_Token_Generated_By_The_AspNetCore_Providers_Should_Not_Validate_Against_The_Abp_Ones()
    {
        // A different options Name means a different Data Protection purpose, so the validating side
        // cannot even unprotect the payload.
        var userId = await CreateUserAsync();

        using var optedOutHost = await CreateHostAsync<OptedOutGeneratorHostModule>();
        try
        {
            var token = await WithUowAsync(optedOutHost, async sp =>
            {
                var userManager = sp.GetRequiredService<IdentityUserManager>();
                var identityOptions = sp.GetRequiredService<IOptions<IdentityOptions>>().Value;
                identityOptions.Tokens.PasswordResetTokenProvider.ShouldBe(TokenOptions.DefaultProvider);
                return await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));
            });

            var verified = await WithUowAsync(ValidatorHost, async sp =>
            {
                var userManager = sp.GetRequiredService<IdentityUserManager>();
                var identityOptions = sp.GetRequiredService<IOptions<IdentityOptions>>().Value;
                var user = await userManager.GetByIdAsync(userId);

                // Store the hash the ABP provider looks for, so a missing hash cannot be the reason
                // this fails. What is left is the protector purpose, which differs with the options Name.
                var storedName = identityOptions.Tokens.PasswordResetTokenProvider + ":" + ResetPasswordPurpose;
                (await userManager.SetAuthenticationTokenAsync(
                    user,
                    AbpSingleActiveTokenProvider.InternalLoginProvider,
                    storedName,
                    Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token))))).Succeeded.ShouldBeTrue();

                user = await userManager.GetByIdAsync(userId);
                (await userManager.GetAuthenticationTokenAsync(
                    user,
                    AbpSingleActiveTokenProvider.InternalLoginProvider,
                    storedName)).ShouldNotBeNull();

                return await userManager.VerifyUserTokenAsync(
                    user,
                    identityOptions.Tokens.PasswordResetTokenProvider,
                    ResetPasswordPurpose,
                    token);
            });

            verified.ShouldBeFalse();
        }
        finally
        {
            await optedOutHost.ShutdownAsync();
        }
    }

    [Fact]
    public async Task A_Token_From_A_Host_That_Registers_The_AspNetCore_Providers_Itself_Should_Not_Validate()
    {
        // A host that never loads the ASP.NET Core integration and wires up the stock providers itself.
        var userId = await CreateUserAsync();

        using var stockHost = await CreateHostAsync<StockProviderGeneratorHostModule>();
        try
        {
            var token = await WithUowAsync(stockHost, async sp =>
            {
                var userManager = sp.GetRequiredService<IdentityUserManager>();
                var identityOptions = sp.GetRequiredService<IOptions<IdentityOptions>>().Value;
                identityOptions.Tokens.PasswordResetTokenProvider.ShouldBe(TokenOptions.DefaultProvider);
                identityOptions.Tokens.ProviderMap[TokenOptions.DefaultProvider].ProviderType
                    .ShouldBe(typeof(DataProtectorTokenProvider<IdentityUser>));
                return await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));
            });

            var verified = await WithUowAsync(ValidatorHost, async sp =>
            {
                var userManager = sp.GetRequiredService<IdentityUserManager>();
                var identityOptions = sp.GetRequiredService<IOptions<IdentityOptions>>().Value;
                var user = await userManager.GetByIdAsync(userId);

                // Store the hash the ABP provider looks for, so a missing hash cannot be the reason
                // this fails. What is left is the protector purpose, which differs with the options Name.
                var storedName = identityOptions.Tokens.PasswordResetTokenProvider + ":" + ResetPasswordPurpose;
                (await userManager.SetAuthenticationTokenAsync(
                    user,
                    AbpSingleActiveTokenProvider.InternalLoginProvider,
                    storedName,
                    Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token))))).Succeeded.ShouldBeTrue();

                user = await userManager.GetByIdAsync(userId);
                (await userManager.GetAuthenticationTokenAsync(
                    user,
                    AbpSingleActiveTokenProvider.InternalLoginProvider,
                    storedName)).ShouldNotBeNull();

                return await userManager.VerifyUserTokenAsync(
                    user,
                    identityOptions.Tokens.PasswordResetTokenProvider,
                    ResetPasswordPurpose,
                    token);
            });

            verified.ShouldBeFalse();
        }
        finally
        {
            await stockHost.ShutdownAsync();
        }
    }

    [Fact]
    public async Task A_Password_Reset_Should_Complete_Across_Two_Hosts_That_Both_Opted_Out()
    {
        // The escape hatch the documentation offers: turning the ABP providers off and registering the
        // ASP.NET Core ones has to leave a working flow, as long as both ends do it.
        var userId = await CreateUserAsync();

        using var generator = await CreateHostAsync<StockProviderGeneratorHostModule>();
        using var validator = await CreateHostAsync<OptedOutGeneratorHostModule>();
        try
        {
            var token = await WithUowAsync(generator, async sp =>
            {
                var userManager = sp.GetRequiredService<IdentityUserManager>();
                var identityOptions = sp.GetRequiredService<IOptions<IdentityOptions>>().Value;
                identityOptions.Tokens.ProviderMap[identityOptions.Tokens.PasswordResetTokenProvider].ProviderType
                    .ShouldBe(typeof(DataProtectorTokenProvider<IdentityUser>));
                return await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));
            });

            var result = await WithUowAsync(validator, async sp =>
            {
                var userManager = sp.GetRequiredService<IdentityUserManager>();
                var identityOptions = sp.GetRequiredService<IOptions<IdentityOptions>>().Value;
                identityOptions.Tokens.ProviderMap[identityOptions.Tokens.PasswordResetTokenProvider].ProviderType
                    .ShouldBe(typeof(DataProtectorTokenProvider<IdentityUser>));
                return await userManager.ResetPasswordAsync(await userManager.GetByIdAsync(userId), token, "1q2w3E*OPTOUT");
            });

            result.Succeeded.ShouldBeTrue();
        }
        finally
        {
            await validator.ShutdownAsync();
            await generator.ShutdownAsync();
        }
    }

    [Fact]
    public async Task Removing_A_Stored_Token_Should_Follow_A_Customized_Provider_Name()
    {
        var userId = await CreateUserAsync();

        using var renamedHost = await CreateHostAsync<CustomProviderNameHostModule>();
        try
        {
            var stillValid = await WithUowAsync(renamedHost, async sp =>
            {
                var userManager = sp.GetRequiredService<IdentityUserManager>();
                var identityOptions = sp.GetRequiredService<IOptions<IdentityOptions>>().Value;

                var token = await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));

                // The hash follows the options Name, while the provider stays on its registration key.
                // Without this the test would also pass if the renaming never took effect.
                var user = await userManager.GetByIdAsync(userId);
                await sp.GetRequiredService<IIdentityUserRepository>().EnsureCollectionLoadedAsync(user, u => u.Tokens);
                identityOptions.Tokens.PasswordResetTokenProvider.ShouldBe(AbpPasswordResetTokenProvider.ProviderName);
                user.FindToken(
                    AbpSingleActiveTokenProvider.InternalLoginProvider,
                    CustomProviderNameHostModule.CustomName + ":" + ResetPasswordPurpose).ShouldNotBeNull();

                (await userManager.RemovePasswordResetTokenAsync(await userManager.GetByIdAsync(userId)))
                    .Succeeded.ShouldBeTrue();

                return await userManager.VerifyUserTokenAsync(
                    await userManager.GetByIdAsync(userId),
                    identityOptions.Tokens.PasswordResetTokenProvider,
                    ResetPasswordPurpose,
                    token);
            });

            stillValid.ShouldBeFalse();
        }
        finally
        {
            await renamedHost.ShutdownAsync();
        }
    }

    [Fact]
    public async Task Removing_A_Stored_Token_Should_Fail_Loudly_When_The_Provider_Is_Not_Abps()
    {
        // Reporting success here would leave the caller believing a token was revoked while the stock
        // provider, which keeps no server side state, happily keeps validating it.
        var userId = await CreateUserAsync();

        using var stockHost = await CreateHostAsync<StockProviderGeneratorHostModule>();
        try
        {
            await WithUowAsync(stockHost, async sp =>
            {
                var userManager = sp.GetRequiredService<IdentityUserManager>();
                var user = await userManager.GetByIdAsync(userId);

                await Should.ThrowAsync<AbpException>(async () => await userManager.RemovePasswordResetTokenAsync(user));
                return true;
            });
        }
        finally
        {
            await stockHost.ShutdownAsync();
        }
    }

    [Fact]
    public async Task Removing_A_Stored_Token_Should_Ignore_A_Provider_Registered_For_Another_User_Type()
    {
        // IdentityUserManager skips a provider that does not serve IdentityUser, so the helpers have to
        // skip it too. The key's public ProviderType is the last type registered under it, which is the
        // other user type's provider here.
        var userId = await CreateUserAsync();

        using var secondUserTypeHost = await CreateHostAsync<SecondUserTypeHostModule>();
        try
        {
            await WithUowAsync(secondUserTypeHost, async sp =>
            {
                var identityOptions = sp.GetRequiredService<IOptions<IdentityOptions>>().Value;
                identityOptions.Tokens.ProviderMap[AbpPasswordResetTokenProvider.ProviderName].ProviderType
                    .ShouldBe(typeof(OtherUserTokenProvider));

                var userManager = sp.GetRequiredService<IdentityUserManager>();

                // What the map says and what the manager uses come apart here.
                userManager.FindTokenProvider(AbpPasswordResetTokenProvider.ProviderName)
                    .ShouldBeOfType<AbpPasswordResetTokenProvider>();

                var user = await userManager.GetByIdAsync(userId);

                await userManager.GeneratePasswordResetTokenAsync(user);
                (await userManager.RemovePasswordResetTokenAsync(await userManager.GetByIdAsync(userId)))
                    .Succeeded.ShouldBeTrue();

                return true;
            });
        }
        finally
        {
            await secondUserTypeHost.ShutdownAsync();
        }
    }

    [Fact]
    public async Task The_Validating_Host_Should_Decide_Whether_A_Token_Expired()
    {
        var userId = await CreateUserAsync();

        var token = await WithUowAsync(GeneratorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));
        });

        // The generating host used the default two hours; this one expires everything immediately.
        using var shortLivedValidator = await CreateHostAsync<ExpiredOnArrivalValidatorHostModule>();
        try
        {
            var verified = await WithUowAsync(shortLivedValidator, async sp =>
            {
                sp.GetRequiredService<IOptions<AbpPasswordResetTokenProviderOptions>>().Value.TokenLifespan
                    .ShouldBe(TimeSpan.Zero);
                sp.GetRequiredService<IOptions<DataProtectionTokenProviderOptions>>().Value.TokenLifespan
                    .ShouldBe(TimeSpan.FromDays(30));

                var userManager = sp.GetRequiredService<IdentityUserManager>();
                var identityOptions = sp.GetRequiredService<IOptions<IdentityOptions>>().Value;
                return await userManager.VerifyUserTokenAsync(
                    await userManager.GetByIdAsync(userId),
                    identityOptions.Tokens.PasswordResetTokenProvider,
                    ResetPasswordPurpose,
                    token);
            });

            verified.ShouldBeFalse();
        }
        finally
        {
            await shortLivedValidator.ShutdownAsync();
        }
    }

    [Fact]
    public async Task The_Generating_Host_Lifespan_Should_Not_Decide_Whether_A_Token_Expired()
    {
        var userId = await CreateUserAsync();

        using var shortLivedGenerator = await CreateHostAsync<ExpiredOnArrivalGeneratorHostModule>();
        string token;
        try
        {
            token = await WithUowAsync(shortLivedGenerator, async sp =>
            {
                // Without this the test would also pass if the zero lifespan never took effect.
                sp.GetRequiredService<IOptions<AbpPasswordResetTokenProviderOptions>>().Value.TokenLifespan
                    .ShouldBe(TimeSpan.Zero);

                var userManager = sp.GetRequiredService<IdentityUserManager>();
                return await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));
            });
        }
        finally
        {
            await shortLivedGenerator.ShutdownAsync();
        }

        var verified = await WithUowAsync(ValidatorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            var identityOptions = sp.GetRequiredService<IOptions<IdentityOptions>>().Value;
            return await userManager.VerifyUserTokenAsync(
                await userManager.GetByIdAsync(userId),
                identityOptions.Tokens.PasswordResetTokenProvider,
                ResetPasswordPurpose,
                token);
        });

        verified.ShouldBeTrue();
    }
}

public abstract class EfCoreCrossHostTestModuleBase : CrossHostTokenProviderTestModuleBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        Configure<AbpDbContextOptions>(options => options.Configure(c => c.UseSqlite()));
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
    }
}

[DependsOn(typeof(AbpAutofacModule), typeof(AbpIdentityEntityFrameworkCoreModule), typeof(AbpEntityFrameworkCoreSqliteModule))]
public class CrossHostGeneratorHostModule : EfCoreCrossHostTestModuleBase
{
}

[DependsOn(typeof(AbpAutofacModule), typeof(AbpIdentityEntityFrameworkCoreModule), typeof(AbpIdentityAspNetCoreModule), typeof(AbpEntityFrameworkCoreSqliteModule))]
public class CrossHostValidatorHostModule : EfCoreCrossHostTestModuleBase
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityAspNetCoreOptions>(options => options.ConfigureAuthentication = false);
    }
}

/// A host that opted out of the ABP providers and registered the ASP.NET Core ones instead.
[DependsOn(typeof(AbpAutofacModule), typeof(AbpIdentityEntityFrameworkCoreModule), typeof(AbpIdentityAspNetCoreModule), typeof(AbpEntityFrameworkCoreSqliteModule))]
public class OptedOutGeneratorHostModule : EfCoreCrossHostTestModuleBase
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityAspNetCoreOptions>(options => options.ConfigureAuthentication = false);
        PreConfigure<AbpIdentityTokenProviderOptions>(options => options.UseAbpTokenProviders = false);
        PreConfigure<IdentityBuilder>(builder => builder.AddDefaultTokenProviders());
    }
}

/// Validator whose password-reset lifespan is zero. It also sets the ASP.NET Core
/// <c>DataProtectionTokenProviderOptions</c> to 30 days, which must not rescue the token: the ABP
/// providers read their own options type.
[DependsOn(typeof(AbpAutofacModule), typeof(AbpIdentityEntityFrameworkCoreModule), typeof(AbpIdentityAspNetCoreModule), typeof(AbpEntityFrameworkCoreSqliteModule))]
public class ExpiredOnArrivalValidatorHostModule : EfCoreCrossHostTestModuleBase
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityAspNetCoreOptions>(options => options.ConfigureAuthentication = false);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        Configure<AbpPasswordResetTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.Zero);
        Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(30));
    }
}

/// Generator whose password-reset lifespan is zero. The validating host still decides.
[DependsOn(typeof(AbpAutofacModule), typeof(AbpIdentityEntityFrameworkCoreModule), typeof(AbpEntityFrameworkCoreSqliteModule))]
public class ExpiredOnArrivalGeneratorHostModule : EfCoreCrossHostTestModuleBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        Configure<AbpPasswordResetTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.Zero);
    }
}

/// A host that never loads the ASP.NET Core integration and registers the stock providers itself.
[DependsOn(typeof(AbpAutofacModule), typeof(AbpIdentityEntityFrameworkCoreModule), typeof(AbpEntityFrameworkCoreSqliteModule))]
public class StockProviderGeneratorHostModule : EfCoreCrossHostTestModuleBase
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityTokenProviderOptions>(options => options.UseAbpTokenProviders = false);
        PreConfigure<IdentityBuilder>(builder => builder.AddDefaultTokenProviders());
    }
}

/// A host where a second Identity user type registered a provider under an ABP key. The descriptor
/// hands that one out, while the manager keeps using the ABP provider.
[DependsOn(typeof(AbpAutofacModule), typeof(AbpIdentityEntityFrameworkCoreModule), typeof(AbpEntityFrameworkCoreSqliteModule))]
public class SecondUserTypeHostModule : EfCoreCrossHostTestModuleBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        // After the framework registration, so this one ends up on top of the key's provider stack.
        new IdentityBuilder(typeof(OtherUser), context.Services)
            .AddTokenProvider<OtherUserTokenProvider>(AbpPasswordResetTokenProvider.ProviderName);
    }
}

public class OtherUser
{
}

public class OtherUserTokenProvider : IUserTwoFactorTokenProvider<OtherUser>
{
    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<OtherUser> manager, OtherUser user) => Task.FromResult(false);

    public Task<string> GenerateAsync(string purpose, UserManager<OtherUser> manager, OtherUser user) => Task.FromResult(string.Empty);

    public Task<bool> ValidateAsync(string purpose, string token, UserManager<OtherUser> manager, OtherUser user) => Task.FromResult(false);
}

/// A host that renamed the password-reset provider. The stored hash then lives under that name and
/// no longer matches the key the provider is registered under.
[DependsOn(typeof(AbpAutofacModule), typeof(AbpIdentityEntityFrameworkCoreModule), typeof(AbpEntityFrameworkCoreSqliteModule))]
public class CustomProviderNameHostModule : EfCoreCrossHostTestModuleBase
{
    public const string CustomName = "CustomPasswordReset";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        Configure<AbpPasswordResetTokenProviderOptions>(options => options.Name = CustomName);
    }
}
