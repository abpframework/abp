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
}
