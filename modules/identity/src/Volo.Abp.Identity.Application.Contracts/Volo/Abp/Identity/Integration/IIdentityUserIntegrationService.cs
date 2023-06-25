using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity.Integration;

[IntegrationService]
public interface IIdentityUserIntegrationService : IApplicationService
{
    Task<string[]> GetRoleNamesAsync(Guid id);
}