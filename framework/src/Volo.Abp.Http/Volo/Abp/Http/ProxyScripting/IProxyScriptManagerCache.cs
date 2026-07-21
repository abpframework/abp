using System;
using System.Threading.Tasks;

namespace Volo.Abp.Http.ProxyScripting;

public interface IProxyScriptManagerCache
{
    Task<string> GetOrAddAsync(string key, Func<Task<string>> factory);
}
