#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Shouldly;
using Xunit;

namespace Volo.Abp.Http.Client.ClientProxying;

public class ClientProxyBase_AccessTokenHash_Tests
{
    [Fact]
    public void Should_Hash_The_Bearer_Token_Of_An_Unauthorized_Response()
    {
        var response = CreateResponse(HttpStatusCode.Unauthorized, new AuthenticationHeaderValue("Bearer", "token-a"));

        new TestableClientProxy().PublicGetAccessTokenHash(response).ShouldBe("token-a".ToSha256());
    }

    [Fact]
    public void Should_Match_The_Bearer_Scheme_Case_Insensitively()
    {
        var response = CreateResponse(HttpStatusCode.Unauthorized, new AuthenticationHeaderValue("bearer", "token-a"));

        new TestableClientProxy().PublicGetAccessTokenHash(response).ShouldBe("token-a".ToSha256());
    }

    [Theory]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public void Should_Return_Null_For_Other_Status_Codes(HttpStatusCode statusCode)
    {
        var response = CreateResponse(statusCode, new AuthenticationHeaderValue("Bearer", "token-a"));

        new TestableClientProxy().PublicGetAccessTokenHash(response).ShouldBeNull();
    }

    [Fact]
    public void Should_Return_Null_For_Other_Authentication_Schemes()
    {
        var response = CreateResponse(HttpStatusCode.Unauthorized, new AuthenticationHeaderValue("Basic", "dXNlcjpwYXNz"));

        new TestableClientProxy().PublicGetAccessTokenHash(response).ShouldBeNull();
    }

    [Fact]
    public void Should_Return_Null_Without_A_Token()
    {
        var proxy = new TestableClientProxy();

        proxy.PublicGetAccessTokenHash(CreateResponse(HttpStatusCode.Unauthorized, new AuthenticationHeaderValue("Bearer", ""))).ShouldBeNull();
        proxy.PublicGetAccessTokenHash(CreateResponse(HttpStatusCode.Unauthorized, null)).ShouldBeNull();
        proxy.PublicGetAccessTokenHash(new HttpResponseMessage(HttpStatusCode.Unauthorized)).ShouldBeNull();
    }

    private static HttpResponseMessage CreateResponse(HttpStatusCode statusCode, AuthenticationHeaderValue? authorization)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/api/test");
        request.Headers.Authorization = authorization;
        return new HttpResponseMessage(statusCode) { RequestMessage = request };
    }

    private sealed class TestableClientProxy : ClientProxyBase<object>
    {
        public string? PublicGetAccessTokenHash(HttpResponseMessage response)
        {
            return GetAccessTokenHash(response);
        }
    }
}
