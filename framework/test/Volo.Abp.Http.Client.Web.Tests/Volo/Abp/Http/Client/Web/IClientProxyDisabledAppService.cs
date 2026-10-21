using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Http.Client.Web;

public interface IClientProxyDisabledAppService : IApplicationService
{
    Task<string> GetAsync();

    Task<string> GetWithRouteAsync();
}
