using MongoDB.Driver;
using Volo.Abp.EntityFrameworkCore.TestApp.FourthContext;

namespace Volo.Abp.MongoDB.TestApp.FourthContext
{
    /* This dbcontext is just for testing to replace dbcontext from the application using ReplaceDbContextAttribute
     */
    public class FourthDbContext : AbpMongoDbContext, IFourthDbContext
    {
        public IMongoCollection<FourthDbContextDummyEntity> FourthDummyEntities => Collection<FourthDbContextDummyEntity>();

    }
}
