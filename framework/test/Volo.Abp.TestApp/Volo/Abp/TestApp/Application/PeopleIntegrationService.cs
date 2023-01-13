using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TestApp.Application;

[IntegrationService]
public class PeopleIntegrationService : ApplicationService, IPeopleIntegrationService
{
    public async Task<string> GetValueAsync()
    {
        return "42";
    }
}