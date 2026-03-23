using System.Threading.Tasks;

namespace Volo.Abp.Http.ProxyScripting;

public interface IProxyScriptManager
{
    Task<string> GetScriptAsync(ProxyScriptingModel scriptingModel);
}
