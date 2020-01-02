using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.Devices
{
    public interface IDeviceFlowCodesRepository : IBasicRepository<DeviceFlowCodes, Guid>
    {

    }
}
