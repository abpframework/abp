using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Http.Client.Web;

public interface IClientProxyDisabledActionAppService : IApplicationService
{
    Task<string> GetAsync();
}
