using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Http;
using Xunit;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public class ClientProxyExceptionEventHandler_Tests
{
    [Fact]
    public async Task Should_Not_Sign_Out_On_401_For_A_Token_The_Client_Revoked()
    {
        var token = "revoked-" + Guid.NewGuid();
        WebAssemblyRevokedAccessTokens.TryAdd(token);

        try
        {
            var navigation = await HandleAsync(401, token.ToSha256());

            navigation.Uris.ShouldBeEmpty();
        }
        finally
        {
            WebAssemblyRevokedAccessTokens.Remove(token);
        }
    }

    [Fact]
    public async Task Should_Sign_Out_On_401_For_Any_Other_Token()
    {
        var token = "revoked-" + Guid.NewGuid();
        WebAssemblyRevokedAccessTokens.TryAdd(token);

        try
        {
            var navigation = await HandleAsync(401, ("current-" + Guid.NewGuid()).ToSha256());

            navigation.Uris.ShouldHaveSingleItem().ShouldContain("authentication/logout");
        }
        finally
        {
            WebAssemblyRevokedAccessTokens.Remove(token);
        }
    }

    [Fact]
    public async Task Should_Sign_Out_On_401_Without_A_Token()
    {
        var navigation = await HandleAsync(401, null);

        navigation.Uris.ShouldHaveSingleItem().ShouldContain("authentication/logout");
    }

    [Fact]
    public async Task Should_Ignore_Other_Status_Codes()
    {
        var navigation = await HandleAsync(403, ("current-" + Guid.NewGuid()).ToSha256());

        navigation.Uris.ShouldBeEmpty();
    }

    private static async Task<RecordingNavigationManager> HandleAsync(int statusCode, string? accessTokenHash)
    {
        var navigation = new RecordingNavigationManager();
        var services = new ServiceCollection();
        services.AddOptions();
        services.Configure<AbpAspNetCoreComponentsWebOptions>(options => options.IsBlazorWebApp = false);
        services.Configure<AbpAuthenticationOptions>(options => options.LogoutUrl = "authentication/logout");
        services.AddSingleton<NavigationManager>(navigation);

        await using var serviceProvider = services.BuildServiceProvider();
        var handler = new ClientProxyExceptionEventHandler(serviceProvider);
        await handler.HandleEventAsync(new ClientProxyExceptionEventData
        {
            StatusCode = statusCode,
            AccessTokenHash = accessTokenHash
        });

        return navigation;
    }

    private sealed class RecordingNavigationManager : NavigationManager
    {
        public List<string> Uris { get; } = new();

        public RecordingNavigationManager()
        {
            Initialize("https://app.example.test/", "https://app.example.test/");
        }

        protected override void NavigateToCore(string uri, NavigationOptions options)
        {
            Uris.Add(uri);
        }
    }
}
