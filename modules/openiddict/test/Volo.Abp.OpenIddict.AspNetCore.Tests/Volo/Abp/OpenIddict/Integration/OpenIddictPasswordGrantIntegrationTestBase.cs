using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.Integration;

public abstract class OpenIddictPasswordGrantIntegrationTestBase : AbpWebApplicationFactoryIntegratedTest<Program>
{
    protected const string NewPassword = "2q3w4E*";

    protected Task<HttpResponseMessage> RequestPasswordTokenAsync(
        Dictionary<string, string> additionalParameters = null,
        string password = null)
    {
        var parameters = new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = "test-client",
            ["client_secret"] = "test-secret",
            ["username"] = OpenIddictPasswordGrantTestData.UserName,
            ["password"] = password ?? OpenIddictPasswordGrantTestData.Password
        };

        if (additionalParameters != null)
        {
            foreach (var parameter in additionalParameters)
            {
                parameters[parameter.Key] = parameter.Value;
            }
        }

        return Client.PostAsync("/connect/token", new FormUrlEncodedContent(parameters));
    }

    protected static async Task AssertAccessTokenAsync(HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        document.RootElement.GetProperty("access_token").GetString().ShouldNotBeNullOrWhiteSpace();
    }

    protected static async Task AssertInvalidGrantAsync(HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;
        root.GetProperty("error").GetString().ShouldBe("invalid_grant");
        root.TryGetProperty("access_token", out _).ShouldBeFalse();
    }

    protected Task<string> GenerateTwoFactorCodeAsync()
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
            return await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
        });
    }

    protected Task<int> GetAccessFailedCountAsync()
    {
        return WithUnitOfWorkAsync(
            new AbpUnitOfWorkOptions { IsTransactional = false },
            async serviceProvider =>
            {
                var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
                var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
                return await userManager.GetAccessFailedCountAsync(user);
            });
    }

    protected virtual Task WithUnitOfWorkAsync(Func<IServiceProvider, Task> action)
    {
        return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), action);
    }

    protected virtual async Task WithUnitOfWorkAsync(
        AbpUnitOfWorkOptions options,
        Func<IServiceProvider, Task> action)
    {
        using var scope = ServiceProvider.CreateScope();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin(options);
        await action(scope.ServiceProvider);
        await uow.CompleteAsync();
    }

    protected virtual Task<TResult> WithUnitOfWorkAsync<TResult>(Func<IServiceProvider, Task<TResult>> action)
    {
        return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), action);
    }

    protected virtual async Task<TResult> WithUnitOfWorkAsync<TResult>(
        AbpUnitOfWorkOptions options,
        Func<IServiceProvider, Task<TResult>> action)
    {
        using var scope = ServiceProvider.CreateScope();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin(options);
        var result = await action(scope.ServiceProvider);
        await uow.CompleteAsync();
        return result;
    }
}
