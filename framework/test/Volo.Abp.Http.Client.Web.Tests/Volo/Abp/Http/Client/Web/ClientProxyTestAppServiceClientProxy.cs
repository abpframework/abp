using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Abp.Http.Client.ClientProxying;

namespace Volo.Abp.Http.Client.Web;

// Returns the received values instead of calling a remote service, so the tests can see what the proxy controller bound.
public class ClientProxyTestAppServiceClientProxy : ClientProxyBase<IClientProxyTestAppService>, IClientProxyTestAppService
{
    public virtual Task<string> GetFromBodyAsync(string value)
    {
        return Task.FromResult(value);
    }

    public virtual Task<string> GetFromQueryAsync(string value)
    {
        return Task.FromResult(value);
    }

    public virtual Task<int> GetNumberFromBodyAsync(int value)
    {
        return Task.FromResult(value);
    }

    public virtual Task<string> GetFromExplicitQueryAsync([FromQuery] string value)
    {
        return Task.FromResult(value);
    }

    public virtual Task<string> GetQueryParametersAsync(int number, bool flag, Guid guid)
    {
        return Task.FromResult($"{number}:{flag}:{guid}");
    }

    public virtual Task<string> CreateAsync(ClientProxyTestInput input)
    {
        return Task.FromResult($"{input.Name}:{input.Value}");
    }

    public virtual Task<string> UpdateAsync(Guid id, ClientProxyTestInput input)
    {
        return Task.FromResult($"{id}:{input.Name}:{input.Value}");
    }

    public virtual Task<string> GetListAsync(ClientProxyTestInput input)
    {
        return Task.FromResult($"{input.Name}:{input.Value}");
    }

    public virtual Task<string> GetByQueryTypeAsync(ClientProxyTestQueryInput input)
    {
        return Task.FromResult($"{input.Name}:{input.Value}");
    }

    public virtual async Task<string> UploadAsync(IRemoteStreamContent file)
    {
        using var reader = new StreamReader(file.GetStream());
        return $"{file.FileName}:{await reader.ReadToEndAsync()}";
    }

    [RemoteService(IsMetadataEnabled = false)]
    public virtual Task<string> GetHiddenAsync()
    {
        return Task.FromResult("hidden");
    }

    public virtual Task<string> GetNotDescribedAsync()
    {
        return Task.FromResult("not-described");
    }

    [RemoteService(false)]
    public virtual Task<string> GetDisabledAsync()
    {
        return Task.FromResult("disabled");
    }

    [RemoteService(false)]
    [HttpGet]
    public virtual Task<string> GetDisabledWithVerbAsync()
    {
        return Task.FromResult("disabled-with-verb");
    }

    [RemoteService]
    public virtual Task<string> GetNotDescribedWithRemoteServiceAsync()
    {
        return Task.FromResult("not-described-with-remote-service");
    }

    [HttpGet]
    public virtual Task<string> GetNotDescribedWithVerbAsync()
    {
        return Task.FromResult("not-described-with-verb");
    }

    [RemoteService(IsMetadataEnabled = false)]
    public virtual Task<string> GetNotDescribedHiddenAsync()
    {
        return Task.FromResult("not-described-hidden");
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public virtual Task<string> GetNotDescribedIgnoredAsync()
    {
        return Task.FromResult("not-described-ignored");
    }

    [RemoteService(false)]
    [HttpGet("api/client-proxy-test/disabled-with-route")]
    public virtual Task<string> GetDisabledWithRouteAsync()
    {
        return Task.FromResult("disabled-with-route");
    }

    [HttpGet("api/client-proxy-test/not-described-with-route")]
    public virtual Task<string> GetNotDescribedWithRouteAsync()
    {
        return Task.FromResult("not-described-with-route");
    }

    [RemoteService(false)]
    [HttpGet]
    [HttpPost("api/client-proxy-test/disabled-with-verb-and-route")]
    public virtual Task<string> GetDisabledWithVerbAndRouteAsync()
    {
        return Task.FromResult("disabled-with-verb-and-route");
    }
}
