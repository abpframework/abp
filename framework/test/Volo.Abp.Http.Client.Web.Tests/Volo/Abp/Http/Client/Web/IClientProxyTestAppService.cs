using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.Abp.Http.Client.Web;

public interface IClientProxyTestAppService : IApplicationService
{
    Task<string> GetFromBodyAsync(string value);

    Task<string> GetFromQueryAsync(string value);

    Task<int> GetNumberFromBodyAsync(int value);

    Task<string> GetFromExplicitQueryAsync(string value);

    Task<string> GetQueryParametersAsync(int number, bool flag, Guid guid);

    Task<string> CreateAsync(ClientProxyTestInput input);

    Task<string> UpdateAsync(Guid id, ClientProxyTestInput input);

    Task<string> GetListAsync(ClientProxyTestInput input);

    Task<string> GetByQueryTypeAsync(ClientProxyTestQueryInput input);

    Task<string> UploadAsync(IRemoteStreamContent file);

    Task<string> GetHiddenAsync();

    Task<string> GetNotDescribedAsync();
}
