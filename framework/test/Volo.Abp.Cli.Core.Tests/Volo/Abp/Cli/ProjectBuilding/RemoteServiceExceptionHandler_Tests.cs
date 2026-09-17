using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Xunit;

namespace Volo.Abp.Cli.ProjectBuilding;

public class RemoteServiceExceptionHandler_Tests
{
    private readonly RemoteServiceExceptionHandler _handler;

    public RemoteServiceExceptionHandler_Tests()
    {
        var jsonSerializer = new AbpSystemTextJsonSerializer(
            Microsoft.Extensions.Options.Options.Create(new AbpSystemTextJsonSerializerOptions())
        );
        _handler = new RemoteServiceExceptionHandler(jsonSerializer);
    }

    [Fact]
    public async Task EnsureSuccessfulHttpResponseAsync_Should_Not_Throw_On_Success()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}")
        };

        await _handler.EnsureSuccessfulHttpResponseAsync(response);
    }

    [Fact]
    public async Task EnsureSuccessfulHttpResponseAsync_Should_Not_Throw_When_Response_Is_Null()
    {
        await _handler.EnsureSuccessfulHttpResponseAsync(null);
    }

    [Fact]
    public async Task Should_Wrap_Html_Body_Without_Json_Parse_Exception()
    {
        var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
        {
            ReasonPhrase = "Forbidden",
            Content = new StringContent("<!DOCTYPE html><html><body>Forbidden</body></html>", System.Text.Encoding.UTF8, "text/html")
        };

        var exception = await Should.ThrowAsync<Exception>(() => _handler.EnsureSuccessfulHttpResponseAsync(response));

        exception.Message.ShouldContain("403-Forbidden");
        exception.Message.ShouldNotContain("invalid start of a value");
    }

    [Fact]
    public async Task Should_Surface_Server_Error_Message_When_Body_Is_Valid_Json()
    {
        var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
        {
            ReasonPhrase = "Forbidden",
            Content = new StringContent(
                "{\"error\":{\"code\":\"LicenseExpired\",\"message\":\"Your ABP license has expired.\"}}",
                System.Text.Encoding.UTF8,
                "application/json")
        };

        var exception = await Should.ThrowAsync<Exception>(() => _handler.EnsureSuccessfulHttpResponseAsync(response));

        exception.Message.ShouldContain("403-Forbidden");
        exception.Message.ShouldContain("LicenseExpired");
        exception.Message.ShouldContain("Your ABP license has expired.");
    }

    [Fact]
    public async Task Should_Surface_Server_Error_Message_For_5xx_With_Json_Body()
    {
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            ReasonPhrase = "Internal Server Error",
            Content = new StringContent(
                "{\"error\":{\"code\":\"InternalError\",\"message\":\"Database connection failed\"}}",
                System.Text.Encoding.UTF8,
                "application/json")
        };

        var exception = await Should.ThrowAsync<Exception>(() => _handler.EnsureSuccessfulHttpResponseAsync(response));

        exception.Message.ShouldContain("500-Internal Server Error");
        exception.Message.ShouldContain("InternalError");
        exception.Message.ShouldContain("Database connection failed");
    }

    [Fact]
    public async Task GetAbpRemoteServiceErrorAsync_Should_Propagate_OperationCanceledException()
    {
        var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
        {
            Content = new CanceledStringContent()
        };

        await Should.ThrowAsync<OperationCanceledException>(
            () => _handler.GetAbpRemoteServiceErrorAsync(response)
        );
    }

    [Fact]
    public async Task GetAbpRemoteServiceErrorAsync_Should_Return_Null_For_Html_Body()
    {
        var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
        {
            Content = new StringContent("<!DOCTYPE html><html></html>", System.Text.Encoding.UTF8, "text/html")
        };

        var result = await _handler.GetAbpRemoteServiceErrorAsync(response);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetAbpRemoteServiceErrorAsync_Should_Return_Null_For_Newtonsoft_JsonException()
    {
        var handler = new RemoteServiceExceptionHandler(
            new ThrowingJsonSerializer(new Newtonsoft.Json.JsonException("Invalid JSON"))
        );
        var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
        {
            Content = new StringContent("{}")
        };

        var result = await handler.GetAbpRemoteServiceErrorAsync(response);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetAbpRemoteServiceErrorAsync_Should_Propagate_Non_Json_Exceptions()
    {
        var handler = new RemoteServiceExceptionHandler(
            new ThrowingJsonSerializer(new InvalidOperationException("Unexpected serializer failure"))
        );
        var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
        {
            Content = new StringContent("{}")
        };

        var exception = await Should.ThrowAsync<InvalidOperationException>(
            () => handler.GetAbpRemoteServiceErrorAsync(response)
        );

        exception.Message.ShouldBe("Unexpected serializer failure");
    }

    private class CanceledStringContent : HttpContent
    {
        protected override Task SerializeToStreamAsync(System.IO.Stream stream, System.Net.TransportContext context)
        {
            throw new OperationCanceledException();
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }

    private class ThrowingJsonSerializer : IJsonSerializer
    {
        private readonly Exception _exception;

        public ThrowingJsonSerializer(Exception exception)
        {
            _exception = exception;
        }

        public string Serialize(object obj, bool camelCase = true, bool indented = false)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(string jsonString, bool camelCase = true)
        {
            throw _exception;
        }

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
        {
            throw _exception;
        }
    }
}
