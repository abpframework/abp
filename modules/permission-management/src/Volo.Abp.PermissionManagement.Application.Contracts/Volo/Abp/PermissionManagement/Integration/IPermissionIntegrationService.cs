using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.PermissionManagement.Integration;

[IntegrationService]
public interface IPermissionIntegrationService : IApplicationService
{
    Task<IsGrantedOutput> IsGrantedAsync(IsGrantedInput input);

    Task<List<IsGrantedOutput>> IsGrantedAsync(List<IsGrantedInput> input);
}
