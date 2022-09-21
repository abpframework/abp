using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TestApp.Application;

public interface IPeopleIntegrationService : IApplicationService
{
    Task<string> GetValueAsync();
}