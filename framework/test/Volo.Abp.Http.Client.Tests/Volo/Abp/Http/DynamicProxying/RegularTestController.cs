using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Http.DynamicProxying;

[Route("api/regular-test-controller")]
[RemoteService] //Automatically enables API explorer and apply ABP conventions.
//[ApiExplorerSettings(IgnoreApi = false)] //alternative
public class RegularTestController : AbpController, IRegularTestController
{
    [HttpGet]
    [Route("increment")]
    public Task<int> IncrementValueAsync(int value)
    {
        return Task.FromResult(value + 1);
    }

    [HttpGet]
    [Route("get-exception1")]
    public Task GetException1Async()
    {
        throw new UserFriendlyException("This is an error message!");
    }

    [HttpGet]
    [Route("get-exception2")]
    public Task GetException2Async()
    {
        throw new BusinessException("Volo.Abp.Http.DynamicProxying:10001")
            .WithData("0","TEST");
    }

    [HttpGet]
    [Route("get-with-datetime-parameter")]
    public Task<DateTime> GetWithDateTimeParameterAsync(DateTime dateTime1)
    {
        var culture = CultureInfo.CurrentCulture;
        return Task.FromResult(dateTime1);
    }

    [HttpPost]
    [Route("post-with-header-and-qs")]
    public Task<string> PostValueWithHeaderAndQueryStringAsync([FromHeader] string headerValue, [FromQuery] string qsValue)
    {
        return Task.FromResult(headerValue + "#" + qsValue);
    }

    [HttpPost]
    [Route("post-with-body")]
    public Task<string> PostValueWithBodyAsync([FromBody] string bodyValue)
    {
        return Task.FromResult(bodyValue);
    }

    [HttpPost]
    [Route("post-object-with-body")]
    public Task<Car> PostObjectWithBodyAsync([FromBody] Car bodyValue)
    {
        return Task.FromResult(bodyValue);
    }

    [HttpPost]
    [Route("post-object-with-query")]
    public Task<Car> PostObjectWithQueryAsync(Car bodyValue)
    {
        return Task.FromResult(bodyValue);
    }

    [HttpGet]
    [Route("post-object-with-url/bodyValue")]
    public Task<Car> GetObjectWithUrlAsync(Car bodyValue)
    {
        return Task.FromResult(bodyValue);
    }

    [HttpGet]
    [Route("post-object-and-id-with-url/{id}")]
    public Task<Car> GetObjectandIdAsync(int id, [FromBody] Car bodyValue)
    {
        bodyValue.Year = id;
        return Task.FromResult(bodyValue);
    }

    [HttpGet]
    [Route("post-object-and-first-release-date-with-url/{time:datetime}")]
    public Task<Car> GetObjectandFirstReleaseDateAsync(DateTime time, Car bodyValue)
    {
        bodyValue.FirstReleaseDate = time;
        return Task.FromResult(bodyValue);
    }

    [HttpGet]
    [Route("post-object-and-count-with-url/{count}")]
    public Task<Car> GetObjectandCountAsync(int count, Car bodyValue)
    {
        bodyValue.Year = count;
        return Task.FromResult(bodyValue);
    }

    [HttpGet]
    [Route("post-object-and-id-with-url-and-query/{id}")]
    public Task<Car> GetObjectAndIdWithQueryAsync(int id, Car bodyValue)
    {
        bodyValue.Year = id;
        return Task.FromResult(bodyValue);
    }

    [HttpPut]
    [Route("put-with-body")]
    public Task<string> PutValueWithBodyAsync([FromBody] string bodyValue)
    {
        return Task.FromResult(bodyValue);
    }

    [HttpPut]
    [Route("put-with-header-and-qs")]
    public Task<string> PutValueWithHeaderAndQueryStringAsync([FromHeader] string headerValue, [FromQuery] string qsValue)
    {
        return Task.FromResult(headerValue + "#" + qsValue);
    }

    [HttpPatch]
    [Route("patch-with-header-and-qs")]
    public Task<string> PatchValueWithHeaderAndQueryStringAsync([FromHeader] string headerValue, [FromQuery] string qsValue)
    {
        return Task.FromResult(headerValue + "#" + qsValue);
    }

    [HttpPatch]
    [Route("patch-with-body")]
    public Task<string> PatchValueWithBodyAsync([FromBody] string bodyValue)
    {
        return Task.FromResult(bodyValue);
    }

    [HttpDelete]
    [Route("delete-by-id")]
    public Task<int> DeleteByIdAsync(int id)
    {
        return Task.FromResult(id + 1);
    }

    [HttpGet]
    [Route("abort-request")]
    public async Task<string> AbortRequestAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        return "AbortRequestAsync";
    }
}

public class Car
{
    [FromQuery]
    public int Year { get; set; }

    [FromQuery]
    public string Model { get; set; }

    [FromQuery]
    public DateTime FirstReleaseDate { get; set; }

    [FromQuery]
    public List<string> Colors { get; set; }

    public Car()
    {
        Colors = new List<string>();
    }
}
