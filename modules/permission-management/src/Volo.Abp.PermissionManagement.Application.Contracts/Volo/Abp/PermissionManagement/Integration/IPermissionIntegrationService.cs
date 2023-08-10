using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.PermissionManagement.Integration;

[IntegrationService]
public interface IPermissionIntegrationService : IApplicationService
{
    Task<ListResultDto<PermissionGrantOutput>> IsGrantedAsync(List<PermissionGrantInput> input);
}
