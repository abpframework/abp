using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Http.Client.Web;

public interface IClientProxyNoGroupNameAppService : IApplicationService
{
    Task<string> GetAsync();
}
