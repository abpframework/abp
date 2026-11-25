using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using NSubstitute;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public class WebAssemblyAuthenticationStateProvider_Tests
{
    [Fact]
    public async Task Should_Not_Revoke_Without_A_Current_Token()
    {
        var old = CreateToken("session-old");
        var provider = new TestProvider(currentAccessToken: null);

        await provider.RevokeAsync(old, CreateToken("session-other"));

        provider.Handler.Requests.ShouldBe(0);
        WebAssemblyRevokedAccessTokens.ContainsHash(old.ToSha256()).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Record_The_Old_Token_Before_Revoking_It()
    {
        var current = CreateToken("session-current");
        var old = CreateToken("session-old");
        var provider = new TestProvider(current);
        var recordedBeforeRequest = false;
        provider.Handler.OnRequest = () => recordedBeforeRequest = WebAssemblyRevokedAccessTokens.ContainsHash(old.ToSha256());

        await provider.RevokeAsync(current, old);

        provider.Handler.Requests.ShouldBe(1);
        recordedBeforeRequest.ShouldBeTrue();
        WebAssemblyRevokedAccessTokens.ContainsHash(old.ToSha256()).ShouldBeTrue();
        WebAssemblyRevokedAccessTokens.ContainsHash(current.ToSha256()).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Record_All_Old_Tokens_Before_Revoking_Any()
    {
        var current = CreateToken("session-current");
        var old1 = CreateToken("session-old");
        var old2 = CreateToken("session-old");
        var provider = new TestProvider(current);
        var recordedBeforeFirstRequest = false;
        provider.Handler.OnRequest = () =>
        {
            if (provider.Handler.Requests == 1)
            {
                recordedBeforeFirstRequest = WebAssemblyRevokedAccessTokens.ContainsHash(old1.ToSha256()) &&
                                             WebAssemblyRevokedAccessTokens.ContainsHash(old2.ToSha256());
            }
        };

        await provider.RevokeAsync(current, old1, old2);

        provider.Handler.Requests.ShouldBe(2);
        recordedBeforeFirstRequest.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Forget_The_Old_Token_When_Revocation_Fails()
    {
        var current = CreateToken("session-current");
        var old = CreateToken("session-old");
        var provider = new TestProvider(current);
        provider.Handler.StatusCode = HttpStatusCode.BadRequest;

        await provider.RevokeAsync(current, old);

        provider.Handler.Requests.ShouldBe(1);
        WebAssemblyRevokedAccessTokens.ContainsHash(old.ToSha256()).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Forget_The_Old_Token_When_Revocation_Throws()
    {
        var current = CreateToken("session-current");
        var old = CreateToken("session-old");
        var provider = new TestProvider(current);
        provider.Handler.Exception = new TaskCanceledException();

        await Should.ThrowAsync<OperationCanceledException>(() => provider.RevokeAsync(current, old));

        WebAssemblyRevokedAccessTokens.ContainsHash(old.ToSha256()).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Forget_The_Remaining_Tokens_When_Revocation_Throws()
    {
        var current = CreateToken("session-current");
        var old1 = CreateToken("session-old1");
        var old2 = CreateToken("session-old2");
        var provider = new TestProvider(current);
        provider.Handler.Exception = new TaskCanceledException();

        await Should.ThrowAsync<OperationCanceledException>(() => provider.RevokeAsync(current, old1, old2));

        provider.Handler.Requests.ShouldBe(1);
        WebAssemblyRevokedAccessTokens.ContainsHash(old1.ToSha256()).ShouldBeFalse();
        WebAssemblyRevokedAccessTokens.ContainsHash(old2.ToSha256()).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Not_Record_Any_Token_When_A_Tracked_Token_Cannot_Be_Read()
    {
        var current = CreateToken("session-current");
        var old = CreateToken("session-old");
        var provider = new TestProvider(current);

        await Should.ThrowAsync<ArgumentException>(() => provider.RevokeAsync(current, old, "not-a-jwt"));

        provider.Handler.Requests.ShouldBe(0);
        WebAssemblyRevokedAccessTokens.ContainsHash(old.ToSha256()).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Revoke_A_Token_Only_Once_At_A_Time()
    {
        var current = CreateToken("session-current");
        var old = CreateToken("session-old");
        var provider = new TestProvider(current);
        var release = new TaskCompletionSource();
        provider.Handler.Gate = release.Task;
        provider.Track(current);
        provider.Track(old);

        try
        {
            var first = provider.RevokeTrackedAsync();
            // The second call must not wait for the revocation the first call started.
            await provider.RevokeTrackedAsync().WaitAsync(TimeSpan.FromSeconds(5));

            provider.Handler.Requests.ShouldBe(1);
            WebAssemblyRevokedAccessTokens.ContainsHash(old.ToSha256()).ShouldBeTrue();

            release.SetResult();
            await first;

            provider.Handler.Requests.ShouldBe(1);
            WebAssemblyRevokedAccessTokens.ContainsHash(old.ToSha256()).ShouldBeTrue();
        }
        finally
        {
            release.TrySetResult();
            provider.Untrack();
        }
    }

    [Fact]
    public async Task Should_Not_Revoke_A_Token_Tracked_While_Reading_The_Current_One()
    {
        var current = CreateToken("session-current");
        var old = CreateToken("session-old");
        var newer = CreateToken("session-newer");
        var provider = new TestProvider(current);
        provider.OnFindAccessToken = () => provider.Track(newer);

        await provider.RevokeAsync(current, old);

        provider.Handler.Requests.ShouldBe(1);
        WebAssemblyRevokedAccessTokens.ContainsHash(newer.ToSha256()).ShouldBeFalse();
    }

    private static string CreateToken(string sessionId)
    {
        var token = new JwtSecurityToken(claims: new[]
        {
            new Claim(AbpClaimTypes.SessionId, sessionId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        });

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private sealed class TestProvider
        : WebAssemblyAuthenticationStateProvider<RemoteAuthenticationState, RemoteUserAccount, OidcProviderOptions>
    {
        private readonly string? _currentAccessToken;
        private readonly List<string> _tracked = new();

        public RevocationHandler Handler { get; }

        public Action? OnFindAccessToken { get; set; }

        public TestProvider(string? currentAccessToken)
            : this(currentAccessToken, new RevocationHandler())
        {
        }

        private TestProvider(string? currentAccessToken, RevocationHandler handler)
            : base(
                Substitute.For<IJSRuntime>(),
                CreateOptions(),
                new TestNavigationManager(),
                null!,
                null,
                Substitute.For<WebAssemblyCachedApplicationConfigurationClient>(null, null, null, null, null, null, null, null),
                Microsoft.Extensions.Options.Options.Create(new WebAssemblyAuthenticationStateProviderOptions()),
                CreateHttpClientFactory(handler))
        {
            _currentAccessToken = currentAccessToken;
            Handler = handler;
        }

        protected override Task<string?> FindAccessTokenAsync()
        {
            OnFindAccessToken?.Invoke();
            return Task.FromResult(_currentAccessToken);
        }

        public void Track(string accessToken)
        {
            _tracked.Add(accessToken);
            AccessTokens.TryAdd(accessToken, accessToken);
        }

        public void Untrack()
        {
            foreach (var accessToken in _tracked)
            {
                AccessTokens.TryRemove(accessToken, out _);
            }
        }

        public Task RevokeTrackedAsync() => TryRevokeOldAccessTokensAsync();

        public async Task RevokeAsync(params string[] trackedAccessTokens)
        {
            foreach (var accessToken in trackedAccessTokens)
            {
                Track(accessToken);
            }

            try
            {
                await TryRevokeOldAccessTokensAsync();
            }
            finally
            {
                Untrack();
            }
        }

        private static IOptionsSnapshot<RemoteAuthenticationOptions<OidcProviderOptions>> CreateOptions()
        {
            var options = Substitute.For<IOptionsSnapshot<RemoteAuthenticationOptions<OidcProviderOptions>>>();
            options.Value.Returns(new RemoteAuthenticationOptions<OidcProviderOptions>
            {
                ProviderOptions = { Authority = "https://auth.example.test", ClientId = "test-client" }
            });
            return options;
        }

        private static IHttpClientFactory CreateHttpClientFactory(RevocationHandler handler)
        {
            var factory = Substitute.For<IHttpClientFactory>();
            factory.CreateClient(Arg.Any<string>()).Returns(_ => new HttpClient(handler, disposeHandler: false));
            return factory;
        }
    }

    private sealed class RevocationHandler : HttpMessageHandler
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public Exception? Exception { get; set; }

        public Task? Gate { get; set; }

        public Action? OnRequest { get; set; }

        public int Requests { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests++;
            OnRequest?.Invoke();

            if (Gate != null)
            {
                await Gate;
            }

            if (Exception != null)
            {
                throw Exception;
            }

            return new HttpResponseMessage(StatusCode)
            {
                Content = new StringContent(StatusCode == HttpStatusCode.OK ? "" : "{\"error\":\"invalid_request\"}")
            };
        }
    }

    private sealed class TestNavigationManager : NavigationManager
    {
        public TestNavigationManager()
        {
            Initialize("https://app.example.test/", "https://app.example.test/");
        }
    }
}
