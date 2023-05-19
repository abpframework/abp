using MongoDB.Driver;

namespace Volo.Abp.MongoDB.TestApp.ThirdDbContext;

/* This dbcontext is just for testing to replace dbcontext from the application using AbpDbContextRegistrationOptions.ReplaceDbContext
 */
public class ThirdDbContext : AbpMongoDbContext, IThirdDbContext
{
    public IMongoCollection<ThirdDbContextDummyEntity> DummyEntities => Collection<ThirdDbContextDummyEntity>();
}
