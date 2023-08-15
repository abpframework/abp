using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.PermissionManagement.Integration;

[IntegrationService]
public interface IPermissionIntegrationService : IApplicationService
{
    Task<ListResultDto<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> input);
}
