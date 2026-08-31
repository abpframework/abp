using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

/// <summary>
/// Two independent ABP applications over one shared database and one shared key ring, only one of them
/// loading the ASP.NET Core integration: a token generated on either has to validate on the other. The
/// single-active hash goes through <see cref="IIdentityUserRepository"/>, so every persistence provider
/// derives from this class and runs the same flows against its own database.
/// </summary>
public abstract class CrossHostTokenProvider_Tests<TGeneratorModule, TValidatorModule> : IAsyncLifetime
    where TGeneratorModule : IAbpModule
    where TValidatorModule : IAbpModule
{
    protected const string UserName = "cross-host";
    protected const string TenantUserName = "cross-host-tenant";
    protected const string ResetPasswordPurpose = "ResetPassword";

    protected static readonly Guid TenantId = Guid.Parse("6f6e7b6a-2c1e-4a2f-9d5a-8f2f0f2b7c11");

    protected IAbpApplicationWithInternalServiceProvider GeneratorHost { get; private set; }

    protected IAbpApplicationWithInternalServiceProvider ValidatorHost { get; private set; }

    private IDisposable _database;

    /// <summary>
    /// Creates the database both hosts share and points
    /// <see cref="CrossHostTokenProviderTestModuleBase.ConnectionString"/> at it, before the hosts are built.
    /// </summary>
    protected abstract IDisposable CreateSharedDatabase();

    /// <summary>
    /// The module only the validating host loads. Asserted on both sides so the suite cannot quietly
    /// degrade into two identical hosts, which is not the topology under test.
    /// </summary>
    protected abstract Type ValidatorOnlyModuleType { get; }

    public virtual async Task InitializeAsync()
    {
        _database = CreateSharedDatabase();
        CrossHostTokenProviderTestModuleBase.KeyRepository = new CrossHostInMemoryXmlRepository();

        GeneratorHost = await CreateHostAsync<TGeneratorModule>();
        ValidatorHost = await CreateHostAsync<TValidatorModule>();
    }

    public virtual async Task DisposeAsync()
    {
        if (ValidatorHost != null)
        {
            await ValidatorHost.ShutdownAsync();
            ValidatorHost.Dispose();
        }

        if (GeneratorHost != null)
        {
            await GeneratorHost.ShutdownAsync();
            GeneratorHost.Dispose();
        }

        _database?.Dispose();
    }

    [Fact]
    public void The_Two_Hosts_Should_Be_Independent_Applications()
    {
        GeneratorHost.ServiceProvider.ShouldNotBeSameAs(ValidatorHost.ServiceProvider);

        GeneratorHost.Services.GetSingletonInstance<IModuleContainer>().Modules
            .ShouldNotContain(m => m.Type == ValidatorOnlyModuleType);
        ValidatorHost.Services.GetSingletonInstance<IModuleContainer>().Modules
            .ShouldContain(m => m.Type == ValidatorOnlyModuleType);
    }

    [Fact]
    public void Both_Hosts_Should_Resolve_The_Same_Token_Providers()
    {
        var generatorTokens = GetTokenOptions(GeneratorHost);
        var validatorTokens = GetTokenOptions(ValidatorHost);

        generatorTokens.PasswordResetTokenProvider.ShouldBe(validatorTokens.PasswordResetTokenProvider);
        generatorTokens.EmailConfirmationTokenProvider.ShouldBe(validatorTokens.EmailConfirmationTokenProvider);
        generatorTokens.ChangeEmailTokenProvider.ShouldBe(validatorTokens.ChangeEmailTokenProvider);

        // Both directions: iterating one map alone would pass while the other side has extra keys.
        generatorTokens.ProviderMap.Keys.OrderBy(x => x).ShouldBe(validatorTokens.ProviderMap.Keys.OrderBy(x => x));

        foreach (var (name, descriptor) in validatorTokens.ProviderMap)
        {
            generatorTokens.ProviderMap[name].ProviderType.ShouldBe(descriptor.ProviderType);
        }
    }

    [Fact]
    public async Task A_Token_Generated_On_The_Other_Host_Should_Verify()
    {
        var userId = await CreateUserAsync();

        var token = await WithUowAsync(GeneratorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));
        });

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

    [Fact]
    public async Task A_Token_Generated_On_This_Host_Should_Verify_On_The_Other_One()
    {
        var userId = await CreateUserAsync();

        var token = await WithUowAsync(ValidatorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));
        });

        var verified = await WithUowAsync(GeneratorHost, async sp =>
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

    [Fact]
    public async Task A_Password_Reset_Should_Complete_Across_The_Two_Hosts()
    {
        var userId = await CreateUserAsync();

        var token = await WithUowAsync(GeneratorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));
        });

        var result = await WithUowAsync(ValidatorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.ResetPasswordAsync(await userManager.GetByIdAsync(userId), token, "1q2w3E*NEW");
        });

        result.Succeeded.ShouldBeTrue();
    }

    [Fact]
    public async Task An_Email_Confirmation_Token_Should_Complete_Across_The_Two_Hosts()
    {
        var userId = await CreateUserAsync();

        var token = await WithUowAsync(GeneratorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.GenerateEmailConfirmationTokenAsync(await userManager.GetByIdAsync(userId));
        });

        var result = await WithUowAsync(ValidatorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.ConfirmEmailAsync(await userManager.GetByIdAsync(userId), token);
        });

        result.Succeeded.ShouldBeTrue();
    }

    [Fact]
    public async Task A_Change_Email_Token_Should_Complete_Across_The_Two_Hosts()
    {
        const string newEmail = "changed@abp.io";
        var userId = await CreateUserAsync();

        var token = await WithUowAsync(GeneratorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.GenerateChangeEmailTokenAsync(await userManager.GetByIdAsync(userId), newEmail);
        });

        var result = await WithUowAsync(ValidatorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.ChangeEmailAsync(await userManager.GetByIdAsync(userId), newEmail, token);
        });

        result.Succeeded.ShouldBeTrue();
    }

    public static TheoryData<string, string> OtherSingleActiveProviders => new()
    {
        { TokenOptions.DefaultProvider, "TestPurpose" },
        { LinkUserTokenProviderConsts.LinkUserTokenProviderName, LinkUserTokenProviderConsts.LinkUserTokenPurpose },
    };

    public static TheoryData<string> TwoFactorProviders => new()
    {
        TokenOptions.DefaultEmailProvider,
        TokenOptions.DefaultPhoneProvider,
    };

    [Theory]
    [MemberData(nameof(OtherSingleActiveProviders))]
    public async Task A_Token_Of_The_Other_Single_Active_Providers_Should_Verify_Across_The_Two_Hosts(
        string providerKey,
        string purpose)
    {
        // The password reset flows above go through IdentityOptions.Tokens, which leaves the keys that
        // are addressed directly. They are registered by the same call and break the same way.
        var userId = await CreateUserAsync();

        var token = await WithUowAsync(GeneratorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.GenerateUserTokenAsync(await userManager.GetByIdAsync(userId), providerKey, purpose);
        });

        var verified = await WithUowAsync(ValidatorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.VerifyUserTokenAsync(await userManager.GetByIdAsync(userId), providerKey, purpose, token);
        });

        verified.ShouldBeTrue();
    }

    [Theory]
    [MemberData(nameof(TwoFactorProviders))]
    public async Task A_Two_Factor_Code_Should_Verify_Across_The_Two_Hosts(string providerKey)
    {
        // These keep the code in the user token table too, protected under a purpose built from the
        // provider name, so they need both hosts to agree just as much as the DataProtector ones.
        var userId = await CreateUserAsync();

        var code = await WithUowAsync(GeneratorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.GenerateTwoFactorTokenAsync(await userManager.GetByIdAsync(userId), providerKey);
        });

        var verified = await WithUowAsync(ValidatorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.VerifyTwoFactorTokenAsync(await userManager.GetByIdAsync(userId), providerKey, code);
        });

        verified.ShouldBeTrue();
    }

    [Fact]
    public async Task A_Token_Generated_For_A_Tenant_User_Should_Validate_On_The_Other_Host()
    {
        var userId = await CreateUserAsync(TenantId);

        var token = await WithUowAsync(GeneratorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));
        }, TenantId);

        var result = await WithUowAsync(ValidatorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.ResetPasswordAsync(await userManager.GetByIdAsync(userId), token, "1q2w3E*NEW");
        }, TenantId);

        result.Succeeded.ShouldBeTrue();
    }

    [Fact]
    public async Task The_Stored_Hash_Of_A_Tenant_Users_Token_Should_Carry_The_Tenant_Id()
    {
        // The hash is stored with the user, which is tenant scoped. A host side entry would be
        // invisible to the tenant and the token would be rejected on the validating side.
        var userId = await CreateUserAsync(TenantId);

        await WithUowAsync(GeneratorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            return await userManager.GeneratePasswordResetTokenAsync(await userManager.GetByIdAsync(userId));
        }, TenantId);

        var storedTenantId = await WithUowAsync(ValidatorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            var user = await userManager.GetByIdAsync(userId);
            await sp.GetRequiredService<IIdentityUserRepository>().EnsureCollectionLoadedAsync(user, u => u.Tokens);

            return user.FindToken(
                AbpSingleActiveTokenProvider.InternalLoginProvider,
                AbpPasswordResetTokenProvider.ProviderName + ":" + ResetPasswordPurpose).TenantId;
        }, TenantId);

        storedTenantId.ShouldBe(TenantId);
    }

    protected async Task<Guid> CreateUserAsync(Guid? tenantId = null)
    {
        var userName = tenantId == null ? UserName : TenantUserName;

        return await WithUowAsync(GeneratorHost, async sp =>
        {
            var userManager = sp.GetRequiredService<IdentityUserManager>();
            var existing = await userManager.FindByNameAsync(userName);
            if (existing != null)
            {
                return existing.Id;
            }

            var user = new IdentityUser(Guid.NewGuid(), userName, userName + "@abp.io", tenantId);
            (await userManager.CreateAsync(user, "1q2w3E*")).CheckErrors();
            return user.Id;
        }, tenantId);
    }

    protected static TokenOptions GetTokenOptions(IAbpApplicationWithInternalServiceProvider host)
    {
        return host.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value.Tokens;
    }

    protected static async Task<IAbpApplicationWithInternalServiceProvider> CreateHostAsync<TModule>()
        where TModule : IAbpModule
    {
        var host = await AbpApplicationFactory.CreateAsync<TModule>(options => options.UseAutofac());
        await host.InitializeAsync();
        return host;
    }

    protected static async Task<TResult> WithUowAsync<TResult>(
        IAbpApplicationWithInternalServiceProvider host,
        Func<IServiceProvider, Task<TResult>> action,
        Guid? tenantId = null)
    {
        using var scope = host.ServiceProvider.CreateScope();
        using var tenantScope = scope.ServiceProvider.GetRequiredService<ICurrentTenant>().Change(tenantId);
        var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var unitOfWork = unitOfWorkManager.Begin(new AbpUnitOfWorkOptions { IsTransactional = false }, requiresNew: true);
        var result = await action(scope.ServiceProvider);
        await unitOfWork.CompleteAsync();
        return result;
    }
}

/// <summary>
/// The part of the host configuration that does not depend on the persistence provider. Each provider
/// derives from this and adds its own database configuration.
/// </summary>
public abstract class CrossHostTokenProviderTestModuleBase : AbpModule
{
    public static string ConnectionString;
    public static IXmlRepository KeyRepository;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options => options.ConnectionStrings.Default = ConnectionString);

        // Both hosts share one key ring, the way a distributed deployment does.
        context.Services.AddDataProtection().SetApplicationName("CrossHostTokenProviderTests");
        context.Services.Configure<KeyManagementOptions>(options => options.XmlRepository = KeyRepository);
    }
}

public class CrossHostInMemoryXmlRepository : IXmlRepository
{
    private readonly List<XElement> _elements = new List<XElement>();

    public IReadOnlyCollection<XElement> GetAllElements()
    {
        lock (_elements)
        {
            return _elements.Select(x => new XElement(x)).ToList();
        }
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        lock (_elements)
        {
            _elements.Add(new XElement(element));
        }
    }
}
