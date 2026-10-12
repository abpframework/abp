using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Xunit;

namespace Volo.Abp.OpenIddict.Integration;

public class OpenIddictPasswordGrantTwoFactor_Integration_Tests : OpenIddictPasswordGrantIntegrationTestBase
{
    [Fact]
    public async Task Password_Stage_Should_Not_Reset_Access_Failed_Count()
    {
        var invalidCodeResponse = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = "invalid-code"
        });

        invalidCodeResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await GetAccessFailedCountAsync()).ShouldBe(1);

        var passwordStageResponse = await RequestPasswordTokenAsync();

        passwordStageResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        (await GetAccessFailedCountAsync()).ShouldBe(1);
    }

    [Fact]
    public async Task Valid_TwoFactor_Code_Should_Reset_Access_Failed_Count_Before_Issuing_Token()
    {
        var code = await GenerateTwoFactorCodeAsync();
        await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = "invalid-code"
        });
        (await GetAccessFailedCountAsync()).ShouldBe(1);

        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = code
        });

        await AssertAccessTokenAsync(response);
        (await GetAccessFailedCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Valid_Recovery_Code_Should_Reset_Access_Failed_Count_Before_Issuing_Token()
    {
        var recoveryCode = await GenerateRecoveryCodeAfterFailedAccessAsync();

        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["RecoveryCode"] = recoveryCode
        });

        await AssertAccessTokenAsync(response);
        (await GetAccessFailedCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Reset_Failure_Should_Not_Issue_Token()
    {
        var code = await GenerateTwoFactorCodeAfterFailedAccessAsync();
        var failureSimulator = GetRequiredService<IdentityUserStoreFailureSimulator>();
        failureSimulator.FailAccessFailedCountReset();
        HttpResponseMessage response;
        try
        {
            response = await RequestPasswordTokenAsync(new Dictionary<string, string>
            {
                ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
                ["TwoFactorCode"] = code
            });
        }
        finally
        {
            failureSimulator.Reset();
        }

        await AssertInvalidGrantAsync(response);
        (await GetAccessFailedCountAsync()).ShouldBe(1);
    }

    [Fact]
    public async Task Recovery_Code_Should_Remain_Usable_When_Reset_Failure_Rolls_Back_Request()
    {
        var recoveryCode = await GenerateRecoveryCodeAfterFailedAccessAsync();
        var failureSimulator = GetRequiredService<IdentityUserStoreFailureSimulator>();
        failureSimulator.FailAccessFailedCountReset();
        HttpResponseMessage failedResponse;
        try
        {
            failedResponse = await RequestPasswordTokenAsync(new Dictionary<string, string>
            {
                ["RecoveryCode"] = recoveryCode
            });
        }
        finally
        {
            failureSimulator.Reset();
        }

        await AssertInvalidGrantAsync(failedResponse);
        (await GetAccessFailedCountAsync()).ShouldBe(1);

        var retryResponse = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["RecoveryCode"] = recoveryCode
        });

        await AssertAccessTokenAsync(retryResponse);
        (await GetAccessFailedCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Recovery_Code_Should_Remain_Usable_When_Redemption_Update_Fails()
    {
        var recoveryCode = await GenerateRecoveryCodeAfterFailedAccessAsync();
        var failureSimulator = GetRequiredService<IdentityUserStoreFailureSimulator>();
        failureSimulator.FailAfterSuccessfulUpdates(0);
        HttpResponseMessage failedResponse;
        try
        {
            failedResponse = await RequestPasswordTokenAsync(new Dictionary<string, string>
            {
                ["RecoveryCode"] = recoveryCode
            });
        }
        finally
        {
            failureSimulator.Reset();
        }

        await AssertInvalidGrantAsync(failedResponse);
        (await GetAccessFailedCountAsync()).ShouldBe(1);

        var retryResponse = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["RecoveryCode"] = recoveryCode
        });

        await AssertAccessTokenAsync(retryResponse);
        (await GetAccessFailedCountAsync()).ShouldBe(0);
    }

    private Task<string> GenerateTwoFactorCodeAfterFailedAccessAsync()
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
            (await userManager.AccessFailedAsync(user)).CheckErrors();
            return await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
        });
    }

    private Task<string> GenerateRecoveryCodeAfterFailedAccessAsync()
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
            (await userManager.AccessFailedAsync(user)).CheckErrors();
            var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 1);
            return recoveryCodes.Single();
        });
    }
}
