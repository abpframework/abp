using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.TestApp.FourthContext;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.TestApp.FourthContext;
using Volo.Abp.MongoDB.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MongoDB
{
    [ConnectionStringName("TestApp")]
    [ReplaceDbContext(typeof(IFourthDbContext))]
    public class TestAppMongoDbContext : AbpMongoDbContext, ITestAppMongoDbContext, IThirdDbContext, IFourthDbContext
    {
        [MongoCollection("Persons")] //Intentionally changed the collection name to test it
        public IMongoCollection<Person> People => Collection<Person>();

        public IMongoCollection<EntityWithIntPk> EntityWithIntPks => Collection<EntityWithIntPk>();

        public IMongoCollection<City> Cities => Collection<City>();

        public IMongoCollection<ThirdDbContextDummyEntity> DummyEntities  => Collection<ThirdDbContextDummyEntity>();

        public IMongoCollection<FourthDbContextDummyEntity> FourthDummyEntities => Collection<FourthDbContextDummyEntity>();

        protected internal override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.Entity<City>(b =>
            {
                b.CollectionName = "MyCities";
            });
        }
    }
}
