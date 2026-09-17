using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TestApp.Application;

public class InterfaceOnlyDocumentedAppService : ApplicationService, IInterfaceOnlyDocumentedAppService
{
    public async Task<string> GetMessageAsync(string key)
    {
        return await Task.FromResult(key);
    }
}
