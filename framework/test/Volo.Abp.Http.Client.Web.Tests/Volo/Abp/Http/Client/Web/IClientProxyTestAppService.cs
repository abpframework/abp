using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Http.Client.Web;

public interface IClientProxyTestAppService : IApplicationService
{
    Task<string> GetFromBodyAsync(string value);

    Task<string> GetFromQueryAsync(string value);
}
