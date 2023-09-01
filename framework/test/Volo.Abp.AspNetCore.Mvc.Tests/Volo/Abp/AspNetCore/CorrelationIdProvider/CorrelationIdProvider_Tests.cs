using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc;
using Xunit;

namespace Volo.Abp.AspNetCore.CorrelationIdProvider;

public class CorrelationIdProvider_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task Test()
    {
        // Test AbpCorrelationIdMiddleware without X-Correlation-Id header
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/correlation/404"))
        {
            var response = await Client.SendAsync(requestMessage);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            response.Headers.ShouldContain(x => x.Key == "X-Correlation-Id" && x.Value.First() != null);
        }

        var correlationId = Guid.NewGuid().ToString("N");

        // Test AbpCorrelationIdMiddleware
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/correlation/404"))
        {
            requestMessage.Headers.Add("X-Correlation-Id", correlationId);
            var response = await Client.SendAsync(requestMessage);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            response.Headers.ShouldContain(x => x.Key == "X-Correlation-Id" && x.Value.First() == correlationId);
        }

        // Test AspNetCoreCorrelationIdProvider
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/correlation/get"))
        {
            requestMessage.Headers.Add("X-Correlation-Id", correlationId);
            var response = await Client.SendAsync(requestMessage);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            (await response.Content.ReadAsStringAsync()).ShouldBe(correlationId);
        }
    }
}
