using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Volo.Abp.IdentityServer.Devices
{
    public class DeviceFlowCodesRepository : EfCoreRepository<IIdentityServerDbContext, DeviceFlowCodes, Guid>,
        IDeviceFlowCodesRepository
    {
        public DeviceFlowCodesRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
