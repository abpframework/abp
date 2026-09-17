using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Cli.Http;

public class CliHttpClientExtensions_Tests
{
    [Fact]
    public async Task Should_Not_Enumerate_Delays_When_The_First_Request_Succeeds()
    {
        using var httpClient = new HttpClient(
            new DelegateHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)));

        using var response = await httpClient.GetHttpResponseMessageWithRetryAsync<CliHttpClientExtensions_Tests>(
            "https://abp.io",
            sleepDurations: ThrowWhenEnumerated());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Enumerate_One_Delay_Per_Retry()
    {
        var requestCount = 0;
        var enumeratedDelayCount = 0;
        using var httpClient = new HttpClient(
            new DelegateHttpMessageHandler(_ => new HttpResponseMessage(
                ++requestCount < 3 ? HttpStatusCode.ServiceUnavailable : HttpStatusCode.OK)));

        using var response = await httpClient.GetHttpResponseMessageWithRetryAsync<CliHttpClientExtensions_Tests>(
            "https://abp.io",
            sleepDurations: GetDelays());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        requestCount.ShouldBe(3);
        enumeratedDelayCount.ShouldBe(2);

        IEnumerable<TimeSpan> GetDelays()
        {
            for (var i = 0; i < 3; i++)
            {
                enumeratedDelayCount++;
                yield return TimeSpan.Zero;
            }
        }
    }

    private static IEnumerable<TimeSpan> ThrowWhenEnumerated()
    {
        throw new InvalidOperationException("The delay sequence must remain untouched.");
#pragma warning disable CS0162
        yield break;
#pragma warning restore CS0162
    }

    private sealed class DelegateHttpMessageHandler(
        Func<HttpRequestMessage, HttpResponseMessage> responseFactory) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(responseFactory(request));
        }
    }
}
