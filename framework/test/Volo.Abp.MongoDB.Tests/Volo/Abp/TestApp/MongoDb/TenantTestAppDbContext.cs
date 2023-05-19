using MongoDB.Driver;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.TestApp.FifthContext;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TestApp.MongoDb;

[ReplaceDbContext(typeof(IFifthDbContext), MultiTenancySides.Tenant)]
public class TenantTestAppDbContext : AbpMongoDbContext, IFifthDbContext
{
    public IMongoCollection<FifthDbContextDummyEntity> FifthDbContextDummyEntity => Collection<FifthDbContextDummyEntity>();

    public IMongoCollection<FifthDbContextMultiTenantDummyEntity> FifthDbContextMultiTenantDummyEntity => Collection<FifthDbContextMultiTenantDummyEntity>();
}
