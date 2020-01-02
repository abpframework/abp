using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.Devices
{
    public interface IDeviceFlowCodesRepository : IBasicRepository<DeviceFlowCodes, Guid>
    {
        Task<DeviceFlowCodes> FindByUserCodeAsync(
            string userCode,
            CancellationToken cancellationToken = default
        );
    }
}
