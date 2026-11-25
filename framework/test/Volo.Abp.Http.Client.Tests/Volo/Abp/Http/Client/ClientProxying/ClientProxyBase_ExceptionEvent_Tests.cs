#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;
using Xunit;

namespace Volo.Abp.Http.Client.ClientProxying;

public class ClientProxyBase_ExceptionEvent_Tests : AbpHttpClientTestBase
{
    [Fact]
    public async Task Should_Publish_The_Token_Hash_And_Still_Throw_On_401()
    {
        var eventData = await PublishAsync(HttpStatusCode.Unauthorized, new AuthenticationHeaderValue("Bearer", "token-a"));

        eventData.StatusCode.ShouldBe(401);
        eventData.AccessTokenHash.ShouldBe("token-a".ToSha256());
    }

    [Fact]
    public async Task Should_Not_Publish_A_Token_Hash_For_Other_Status_Codes()
    {
        var eventData = await PublishAsync(HttpStatusCode.Forbidden, new AuthenticationHeaderValue("Bearer", "token-a"));

        eventData.StatusCode.ShouldBe(403);
        eventData.AccessTokenHash.ShouldBeNull();
    }

    private async Task<ClientProxyExceptionEventData> PublishAsync(HttpStatusCode statusCode, AuthenticationHeaderValue authorization)
    {
        ClientProxyExceptionEventData? published = null;
        using var subscription = GetRequiredService<ILocalEventBus>().Subscribe<ClientProxyExceptionEventData>(e =>
        {
            published = e;
            return Task.CompletedTask;
        });

        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/api/test");
        request.Headers.Authorization = authorization;
        var response = new HttpResponseMessage(statusCode) { RequestMessage = request };

        var proxy = new TestableClientProxy { LazyServiceProvider = GetRequiredService<IAbpLazyServiceProvider>() };
        await Should.ThrowAsync<AbpRemoteCallException>(() => proxy.PublicThrowExceptionForResponseAsync(response));

        return published.ShouldNotBeNull();
    }

    private sealed class TestableClientProxy : ClientProxyBase<object>
    {
        public Task PublicThrowExceptionForResponseAsync(HttpResponseMessage response)
        {
            return ThrowExceptionForResponseAsync(response);
        }
    }
}
