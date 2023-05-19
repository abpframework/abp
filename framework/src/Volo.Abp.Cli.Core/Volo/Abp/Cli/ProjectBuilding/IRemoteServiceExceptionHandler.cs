using System.Net.Http;
using System.Threading.Tasks;

namespace Volo.Abp.Cli.ProjectBuilding;

public interface IRemoteServiceExceptionHandler
{
    Task EnsureSuccessfulHttpResponseAsync(HttpResponseMessage responseMessage);

    Task<string> GetAbpRemoteServiceErrorAsync(HttpResponseMessage responseMessage);
}
