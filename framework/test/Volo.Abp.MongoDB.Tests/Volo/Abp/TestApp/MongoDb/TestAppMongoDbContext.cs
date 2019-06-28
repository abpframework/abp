using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MongoDB
{
    [ConnectionStringName("TestApp")]
    public class TestAppMongoDbContext : AbpMongoDbContext, ITestAppMongoDbContext
    {
        [MongoCollection("Persons")] //Intentially changed the collection name to test it
        public IMongoCollection<Person> People => Collection<Person>();

        public IMongoCollection<City> Cities => Collection<City>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.Entity<City>(b =>
            {
                b.CollectionName = "MyCities";
            });
        }
    }
}
