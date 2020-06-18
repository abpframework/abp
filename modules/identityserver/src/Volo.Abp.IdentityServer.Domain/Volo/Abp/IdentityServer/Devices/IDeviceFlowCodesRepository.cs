using System;
using System.Collections.Generic;
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

        Task<DeviceFlowCodes> FindByDeviceCodeAsync(
            string deviceCode,
            CancellationToken cancellationToken = default
        );

        Task<List<DeviceFlowCodes>> GetListByExpirationAsync(
            DateTime maxExpirationDate,
            int maxResultCount,
            CancellationToken cancellationToken = default
        );
    }
}
