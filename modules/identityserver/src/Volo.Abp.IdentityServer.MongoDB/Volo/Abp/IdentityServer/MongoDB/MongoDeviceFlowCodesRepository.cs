using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class MongoDeviceFlowCodesRepository :
        MongoDbRepository<IAbpIdentityServerMongoDbContext, DeviceFlowCodes, Guid>, IDeviceFlowCodesRepository
    {
        public MongoDeviceFlowCodesRepository(
            IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}